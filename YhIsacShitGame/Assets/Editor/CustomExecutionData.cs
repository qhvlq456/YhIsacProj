using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace YhProj.Game.YhEditor
{
    [CustomEditor(typeof(ExecutionData))]
    public class CustomExecutionData : Editor
    {
        ExecutionData executionData;

        GUILayoutOption[] textFiledOptions =
        {
        GUILayout.Width(130),
        GUILayout.MaxWidth(600)
    };

        // 나중에 build icon도 설정 만들자
        // build 할때 keystore라든지 기타등등 셋팅도 넣어야 함
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
            //EditorGUILayout.TextArea(executionData.gameMode.ToString(), textFiledOptions);
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

                // 중복 제거
                defineList = defineList.Distinct().ToList();

                // 현제 나의 enum filed에 존재 하지 않는다면 추가한다.
                if (!defineList.Contains(executionData.defineSymbolType.ToString()))
                {
                    defineList.Add(executionData.defineSymbolType.ToString());
                }

                //문자열 다시 합친후 심볼(디파인) 적용 
#if UNITY_IOS || UNITY_IPHONE
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, string.Join(";", defineList.ToArray()));
#elif UNITY_ANDROID
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, string.Join(";", defineList.ToArray()));
#else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, string.Join(";", defineList.ToArray()));
#endif
                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.Space();

            // 모든 define clear 하는 작업
            if (GUILayout.Button("PlayerSetting Symbol Clear", GUILayout.Height(50)))
            {
                //기존 PlayerSettings 에 scripting Define symbols 값들 ;를 구분으로 잘라서 보관 
                List<string> defineList = symbols.Split(';').ToList();
                // 후에 이상 있을시 변경
                defineList.Clear();
                //문자열 다시 합친후 심볼(디파인) 적용 
#if UNITY_IOS || UNITY_IPHONE
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, string.Join(";", defineList.ToArray()));
#elif UNITY_ANDROID
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, string.Join(";", defineList.ToArray()));
#else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, string.Join(";", defineList.ToArray()));
#endif
                AssetDatabase.SaveAssets();
            }
            // end DefineSymbols button
            #endregion DefineSymbols button

            EditorGUILayout.Space();

            #region Save
            if (GUILayout.Button("Save", GUILayout.Height(50)))
            {
                AssetDatabase.SaveAssets();
            }
            #endregion
            EditorGUILayout.Space();
        }
        /*
         * menuitem등을 통해 에디터에서 설정할 수 있는 환경등을 셋팅
         * ExecutionData
         * 환경설정은 필요 없다 어차피 scriptableobject에서 받아서 사용하면 되기 때문 즉, 빌드를 위해 사용 하면 될 듯 하다
         */
        //[MenuItem("YhProjMenu/Preferences/Execution Preferences (실행환경설정)")]
        //static void ExecutionPreferences()
        //{
        //    if(executionData == null)
        //    {
        //        executionData = AssetDatabase.LoadAssetAtPath<ExecutionData>(executionDataPath);
        //    }

        //    // define symbol을 적용해야 한다.. 받아오는건 너무 낭비라고 생각이 듬
        //}
    }
}
