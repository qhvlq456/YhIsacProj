using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using YhProj;

public class YhProjMenu
{
    // AssetDatabase.LoadAssetAtPath ��ο� �ִ� ù���� ������Ʈ�� ã�� ��ȯ�Ѵ�.
    // ���õ� ������ path ��� : AssetDatabase.GetAssetPath
    private static ExecutionData executionData = null;
    private static UIData uiData = null;

    // ���
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

        if(uiData == null)
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
