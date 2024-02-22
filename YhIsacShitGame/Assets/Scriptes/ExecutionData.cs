using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YhProj;

//[System.Serializable, CreateAssetMenu(fileName = "ExecutionData", menuName = "Execution Scriptable/Execution Data", order = int.MaxValue)]
public class ExecutionData : ScriptableObject
{
    // staic���� �� �� �ν����� â�� ǥ�ð� �ȵ�
    public Define.ServerType serverType;
    public Define.GameMode gameMode;
    public Define.DebugLogeer logType;
    public Define.DefineSymbol defineSymbolType;

    public int version;
    public int gameInfo;
}
