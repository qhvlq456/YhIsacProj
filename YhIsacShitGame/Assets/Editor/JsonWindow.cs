using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System;
using YhProj;

public class JsonWindow : EditorWindow
{
    StageData stageData = new StageData();
    string filePath = "Assets/StreamingAssets/StageData.json"; // ���ϴ� ��η� ����

    Vector2 scrollPosition;

    [MenuItem("YhProjMeunu/JsonWindow")]
    static void Open()
    {
        JsonWindow instance = GetWindow<JsonWindow>();
        instance.titleContent = new GUIContent("Json Window");
        instance.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("JSON Editor", EditorStyles.boldLabel);

        // ������ �Է� �ʵ� // �Ŀ� Ÿ���� ���Ͽ� ���� �� �����̱� ��.. �ٵ� �ʹ� �밡�� �۾��ε� ����� ����?
        EditorGUILayout.Space(10);
        GUILayout.Label("Enter StageData:", EditorStyles.boldLabel);
        stageData.lv = EditorGUILayout.IntField("Level", stageData.lv);
        stageData.stage = EditorGUILayout.IntField("Stage", stageData.stage);
        stageData.xOffset = EditorGUILayout.FloatField("X Offset", stageData.xOffset);
        stageData.zOffset = EditorGUILayout.FloatField("Z Offset", stageData.zOffset);
        EditorGUILayout.Space(50);
        // Ÿ�� �迭 �Է� �ʵ�
        stageData.tileArr = new TileData[stageData.Row, stageData.Col];

        // ��ũ�� ��
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < stageData.Row; i++)
        {
            for (int j = 0; j < stageData.Col; j++)
            {
                if(stageData.tileArr[i,j] == null)
                {
                    stageData.tileArr[i, j] = new TileData();
                }

                GUILayout.Label($"Tile ({i}, {j}):", EditorStyles.boldLabel);

                stageData.tileArr[i, j].name = EditorGUILayout.TextField("Name", stageData.tileArr[i, j].name);
                stageData.tileArr[i, j].index = EditorGUILayout.IntField("Value", stageData.tileArr[i, j].index);
                stageData.tileArr[i, j].direction = (Define.Direction)EditorGUILayout.EnumPopup("Direction", stageData.tileArr[i, j].direction);

                EditorGUILayout.Space(5);
            }
        }

        // ��ũ�Ѻ� ����
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(10);

        // JSON�� ���Ϸ� �����ϴ� ��ư
        if (GUILayout.Button("Save JSON to File"))
        {
            SaveJsonToFile();
        }

        EditorGUILayout.Space(20);

        // ���Ͽ��� JSON�� �о���� ��ư
        if (GUILayout.Button("Load JSON from File"))
        {
            LoadJsonFromFile();
        }
    }

    private void SaveJsonToFile()
    {
        try
        {
            // StageData ��ü�� JSON���� ��ȯ�Ͽ� ���Ͽ� ����
            string json = JsonConvert.SerializeObject(stageData, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, json);
            Debug.Log("JSON saved to file: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving JSON to file: " + e.Message);
        }
    }

    private void LoadJsonFromFile()
    {
        try
        {
            // ���Ͽ��� JSON �а� StageData ��ü�� ������ȭ
            string json = System.IO.File.ReadAllText(filePath);
            stageData = JsonConvert.DeserializeObject<StageData>(json); // EditorStageData�� ������ȭ
            Debug.Log("JSON loaded from file: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading JSON from file: " + e.Message);
        }
    }
}
