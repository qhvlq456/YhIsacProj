using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

[CreateAssetMenu(fileName = "ExecutionData", menuName = "Execution Scriptable/Execution Data", order = int.MaxValue)]
public class ExecutionData : ScriptableObject
{
    // staic으로 둘 때 인스펙터 창에 표시가 안됨
    public Define.ServerType serverType;
    public Define.GameMode gameMode;
    public Define.DebugLogeer logType;
    public Define.DefineSymbol defineSymbolType;

    public int version;
    public int gameInfo;
}
