using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Map;

namespace YhProj.Game
{
    public interface IController
    {
        void Initialize(); // 컨트롤러 초기화 메서드
        void Update(); // 프레임마다 호출되는 업데이트 메서드
        void Dispose(); // 리소스 해제 등의 정리 작업을 위한 메서드
    }
    public interface IMapController : IController
    {
        public List<TileObject> TileObjectList { get; }
        void LoadTile(StageData _stageData);
        void DeleteTile(StageData _stageData);
    }
}
