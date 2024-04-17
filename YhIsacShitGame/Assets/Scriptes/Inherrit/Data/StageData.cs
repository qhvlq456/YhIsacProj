using System.Collections.Generic;

namespace YhProj.Game.Map
{
    [System.Serializable]
    public class StageData : GameData
    {
        public int lv;
        public int stage;
        public int row;
        public int col;

        // 이거 변경 필요
        public List<int> tileIdxList;
        public StageData() { }

        public StageData(int _stage, int _lv, List<int> _tileIdxList)
        {
            stage = _stage;
            lv = _lv;
            tileIdxList = _tileIdxList;
        }
    }
}
