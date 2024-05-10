using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using YhProj.Game.UI;

namespace YhProj.Game.YhEditor
{
    public class YhProjMenu
    {
        // AssetDatabase.LoadAssetAtPath 경로에 있는 첫번재 오브젝트를 찾아 반환한다.
        // 선택된 에셋의 path 얻기 : AssetDatabase.GetAssetPath
        private static ExecutionData executionData = null;
        private static UIData uiData = null;

        // 대기
        [MenuItem("YhProjMenu/Create/Scriptable/Execution Data")]
        public static void CreateExecution()
        {
            executionData = AssetDatabase.LoadAssetAtPath(StaticDefine.SCRIPTABLEOBJECT_PATH + "ExecutionData.asset", typeof(ExecutionData)) as ExecutionData;
            if (executionData == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ExecutionData>(), StaticDefine.SCRIPTABLEOBJECT_PATH + "ExecutionData.asset");
                AssetDatabase.SaveAssets();
            }

            Selection.activeObject = executionData;
        }

        [MenuItem("YhProjMenu/Create/Scriptable/UI Data")]
        public static void CreateUIData()
        {
            executionData = AssetDatabase.LoadAssetAtPath(StaticDefine.SCRIPTABLEOBJECT_PATH + "UIData.asset", typeof(UIData)) as ExecutionData;

            if (uiData == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<UIData>(), StaticDefine.SCRIPTABLEOBJECT_PATH + "UIData.asset");
                AssetDatabase.SaveAssets();
            }

            Selection.activeObject = uiData;
        }

        [MenuItem("YhProjMenu/Test/Test")]
        static void JsonCreate()
        {

        }
    }
}
