using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YhProj;

namespace YhProj.Game
{
    //[System.Serializable, CreateAssetMenu(fileName = "ExecutionData", menuName = "Execution Scriptable/Execution Data", order = int.MaxValue)]
    public class ExecutionData : ScriptableObject
    {
        // staic으로 둘 때 인스펙터 창에 표시가 안됨
        public Define.ServerType serverType;
        public Define.Logger logType;
        public Define.DefineSymbol defineSymbolType;

        public int version;
        public int gameInfo;
    }
}
