using System.Collections;
using System.Collections.Generic;

namespace YhProj.Game.Map
{
    public class MapController
    {
        protected MapManager mapManager;

        public MapController(MapManager _mapManager)
        {
            mapManager = _mapManager;
        }
        public virtual void Load()
        {
            // 에디터 모드에서의 로드 동작
            // 맵 편집 기능 등 추가
        }

        public virtual void Update()
        {
            // 에디터 모드에서의 업데이트 동작
        }

        public virtual void Delete()
        {
            // 에디터 모드에서의 삭제 동작
        }

        public virtual void LoadTile(StageData _stageData)
        {

        }
        public virtual void DeleteTile()
        {

        }

    }
    public class MapController<T> : MapController
    {
        protected List<T> tileList = new List<T>();
        public List<T> TileList
        {
            get
            {
                tileList ??= new List<T>();
                return tileList;
            }
        }

        public T this[int _index]
        {
            get
            {
                return tileList[_index];
            }
            set
            {
                tileList[_index] = value;
            }
        }
        public MapController(MapManager _mapManager) : base(_mapManager)
        {
            mapManager = _mapManager;
        }

    }
}
