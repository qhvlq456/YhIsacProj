using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System;
using YhProj.Game.Map;

namespace YhProj.Game.YhEditor
{
    public class JsonWindow : EditorWindow
    {
        StageData stageData = new StageData();
        string filePath = "Assets/StreamingAssets/StageData.json"; // 원하는 경로로 수정

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

            // 데이터 입력 필드 // 후에 타입을 정하여 변경 할 예정이긴 함.. 근데 너무 노가다 작업인데 방법이 없나?
            EditorGUILayout.Space(10);
            GUILayout.Label("Enter StageData:", EditorStyles.boldLabel);
            stageData.lv = EditorGUILayout.IntField("Level", stageData.lv);
            stageData.stage = EditorGUILayout.IntField("Stage", stageData.stage);
            stageData.xOffset = EditorGUILayout.FloatField("X Offset", stageData.xOffset);
            stageData.zOffset = EditorGUILayout.FloatField("Z Offset", stageData.zOffset);
            EditorGUILayout.Space(50);
            // 타일 배열 입력 필드
            stageData.tileArr = new TileData[stageData.Row, stageData.Col];

            // 스크롤 뷰
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < stageData.Row; i++)
            {
                for (int j = 0; j < stageData.Col; j++)
                {
                    if (stageData.tileArr[i, j] == null)
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

            // 스크롤뷰 종료
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);

            // JSON을 파일로 저장하는 버튼
            if (GUILayout.Button("Save JSON to File"))
            {
                SaveJsonToFile();
            }

            EditorGUILayout.Space(20);

            // 파일에서 JSON을 읽어오는 버튼
            if (GUILayout.Button("Load JSON from File"))
            {
                LoadJsonFromFile();
            }
        }

        private void SaveJsonToFile()
        {
            try
            {
                // StageData 객체를 JSON으로 변환하여 파일에 저장
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
                // 파일에서 JSON 읽고 StageData 객체로 역직렬화
                string json = System.IO.File.ReadAllText(filePath);
                stageData = JsonConvert.DeserializeObject<StageData>(json); // EditorStageData로 역직렬화
                Debug.Log("JSON loaded from file: " + filePath);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading JSON from file: " + e.Message);
            }
        }
    }
}
