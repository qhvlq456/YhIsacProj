using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[CustomEditor(typeof(ExecutionData))]
public class CustomExecutionData: Editor
{
    ExecutionData executionData;

    GUILayoutOption[] textFiledOptions = 
    { 
        GUILayout.Width(130), 
        GUILayout.MaxWidth(600) 
    };

    // ���߿� build icon�� ���� ������
    // build �Ҷ� keystore����� ��Ÿ��� ���õ� �־�� ��
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (executionData == null)
        {
            executionData = target as ExecutionData; //AssetDatabase.LoadAssetAtPath<ExecutionData>(StaticDefine.EXECUTIONDATA_PATH);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        #region LabelField
        EditorGUI.BeginDisabledGroup(true);
        // Game Info
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Game Info");
        EditorGUILayout.TextArea(executionData.gameInfo.ToString(), textFiledOptions);
        EditorGUILayout.EndHorizontal();

        // Game Version
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Game Version");
        EditorGUILayout.TextArea(executionData.version.ToString(), textFiledOptions);
        EditorGUILayout.EndHorizontal();

        // Game Mode
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Game Mode");
        EditorGUILayout.TextArea(executionData.gameMode.ToString(), textFiledOptions);
        EditorGUILayout.EndHorizontal();

        // Game Server
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Game Server");
        EditorGUILayout.TextArea(executionData.serverType.ToString(), textFiledOptions);
        EditorGUILayout.EndHorizontal();

        // Log Type
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Log Type");
        EditorGUILayout.TextArea(executionData.logType.ToString(), textFiledOptions);
        EditorGUILayout.EndHorizontal();

        // Define Symbols
#if UNITY_IOS || UNITY_IPHONE
		string symbols = string.Format("{0}",PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS));
#elif UNITY_ANDROID
        string symbols = string.Format("{0}", PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
#else
		string symbols = string.Empty;
#endif
        symbols = Regex.Replace(symbols, @";+", ";");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Define Symbols");
        EditorGUILayout.TextArea(symbols, textFiledOptions);
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
        #endregion

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        #region DefineSymbols button
        // Start DefineSymbols button
        if (GUILayout.Button("PlayerSetting Symbol Update", GUILayout.Height(50)))
        {            
            List<string> defineList = symbols.Split(';').ToList();

            // �ߺ� ����
            defineList = defineList.Distinct().ToList();
            
            // ���� ���� enum filed�� ���� ���� �ʴ´ٸ� �߰��Ѵ�.
            if(!defineList.Contains(executionData.defineSymbolType.ToString()))
            {
                defineList.Add(executionData.defineSymbolType.ToString());
            }

            //���ڿ� �ٽ� ��ģ�� �ɺ�(������) ���� 
#if UNITY_IOS || UNITY_IPHONE
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, string.Join(";", defineList.ToArray()));
#elif UNITY_ANDROID
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, string.Join(";", defineList.ToArray()));
#else
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, string.Join(";", defineList.ToArray()));
#endif
        }

        EditorGUILayout.Space();

        // ��� define clear �ϴ� �۾�
        if (GUILayout.Button("PlayerSetting Symbol Clear", GUILayout.Height(50)))
        {
            //���� PlayerSettings �� scripting Define symbols ���� ;�� �������� �߶� ���� 
            List<string> defineList = symbols.Split(';').ToList();
            // �Ŀ� �̻� ������ ����
            defineList.Clear();
            //���ڿ� �ٽ� ��ģ�� �ɺ�(������) ���� 
#if UNITY_IOS || UNITY_IPHONE
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, string.Join(";", defineList.ToArray()));
#elif UNITY_ANDROID
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, string.Join(";", defineList.ToArray()));
#else
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, string.Join(";", defineList.ToArray()));
#endif
        }
        // end DefineSymbols button
        #endregion DefineSymbols button

        EditorGUILayout.Space();
    }
    /*
     * menuitem���� ���� �����Ϳ��� ������ �� �ִ� ȯ����� ����
     * ExecutionData
     * ȯ�漳���� �ʿ� ���� ������ scriptableobject���� �޾Ƽ� ����ϸ� �Ǳ� ���� ��, ���带 ���� ��� �ϸ� �� �� �ϴ�
     */
    //[MenuItem("YhProjMenu/Preferences/Execution Preferences (����ȯ�漳��)")]
    //static void ExecutionPreferences()
    //{
    //    if(executionData == null)
    //    {
    //        executionData = AssetDatabase.LoadAssetAtPath<ExecutionData>(executionDataPath);
    //    }

    //    // define symbol�� �����ؾ� �Ѵ�.. �޾ƿ��°� �ʹ� ������ ������ ��
    //}
}
