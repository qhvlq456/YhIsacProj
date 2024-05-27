using System;
using System.Collections.Generic;
using System.Numerics;

namespace YhProj.Game.Map
{
    [Serializable]
    public class StageData : GameData
    {
        public List<int> tileIdxList;
        public List<int> enemyIdxList;

        public Vector3 startPos;
        public Vector3 endPos;

        public int lv;
        public int stage;
        public int row;
        public int col;

        public StageData() { }

        public StageData(int _stage, int _lv, List<int> _tileIdxList)
        {
            stage = _stage;
            lv = _lv;
        }
    }
}
