using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YhProj.Game.Player;

namespace YhProj.Game.Map
{
    /// <summary>
    /// Map에 대한 데이터를 총괄
    /// map을 create, update, delete를 한다
    /// 후에 모드 컨트롤러를 생성하여 다른 모드들에 대한 클래스를 정의 후 모드에 따라 분류하여 로직을 만들어 사용하자 아마 inputmanager보면 알듯?
    /// </summary>
    public class MapManager : BaseManager
    {
        private StageHandler stageHandler;
        public void AddStageData(StageData _stageData) => stageHandler.AddStageData(_stageData);
        public StageData GetStageData(int _stage) => stageHandler.GetStageData(_stage);
        public TileData[,] GetTileArrByStage(int _stage) => stageHandler.GetTileArrByStage(_stage);
        public bool IsConstainsStage(int _stage) => stageHandler.IsConstainsStage(_stage);
        public List<StageData> GetStageDataList() => stageHandler.GetStageDataList();

        public Transform root { get; private set; }

        private TileFactory factory;

        public override void Load()
        {
            stageHandler = new StageHandler();
            stageHandler.DataLoad();

            if (root == null)
            {
                root = GameUtil.AttachObj<Transform>("MapRoot");
                root.transform.position = Vector3.zero;
            }
        }
        // 타일에 대한 delete, update라고 생각하면 안되긴 함...
        public override void Dispose()
        {
            EventMediator.OnLoadSequenceEvent -= LoadPlayerEvent;
            EventMediator.OnPlayerLevelChange -= OnPlayerLevelChange;
        }

        /// <summary>
        /// stage에 맞는 데이터를 update하는 함수
        /// tileobject에 존재하는 tiledata를 업데이트 하고 렌더링을 다시 함
        /// 인포에서 데이터 set하려고 한거임
        /// </summary>
        public override void Update()
        {

        }
        // 후에 ui데이터를 넘겨준다면!?

        #region Tile Load and Delete and Save
        public void LoadStage(int _stage)
        {
            StageData stageData = stageHandler.GetStageData(_stage);

            if (stageData == null) 
            {
                return;
            }

            foreach (var tileData in stageData.tileArr)
            {
                TileObject tileObject = new TileObject();

                switch(tileData.elementType)
                {
                    case ElementType.MINE:
                        tileObject = factory.Create<HeroTileObject>(tileData);
                        break;
                    case ElementType.ENEMY:
                        tileObject = factory.Create<EnemyTileObject>(tileData);
                        break;
                    case ElementType.DECO:
                        tileObject = factory.Create<DecoTileObject>(tileData);
                        break;
                }

                // 위치값 초기화 해야 함
            }
        }
        public void DeleteStage(int _stage)
        {
            stageHandler.DeleteStageData(_stage);
        }
        #endregion
        #region MapManager Event
        // player가 레벨업을 할 때마다 호출
        public void OnPlayerLevelChange(int _level)
        {
            //StageData stageData = stageDataList.Find(x => x.lv == _level);
        }
        public void LoadPlayerEvent(PlayerInfo _playerInfo)
        {
            // 이게 의존성이 인풋에 대한 의존성이 많아지니 계속 의존성이 생기게 됨 그럼으로 인풋의 target을 의존성을 없애는 작업 필요!!
            // int halfIdx = StaticDefine.MAX_CREATE_TILE_NUM / 2;

            // Vector3 targetPos = new Vector3(colLinePoniterDic[halfIdx], 10, rowLinePoniterDic[halfIdx]);

            // 후에 변경할것?
            // Managers.Instance.lookTarget.transform.position = targetPos;
        }
        #endregion
    }

    public sealed class StageHandler : IDataHandler
    {
        private Dictionary<int, StageData> stageDataDic = new Dictionary<int, StageData>();
        public void AddStageData(StageData _stageData)
        {
            if (IsConstainsStage(_stageData.stage))
            {
                stageDataDic[_stageData.stage] = _stageData;
            }
            else
            {
                stageDataDic.Add(_stageData.stage, _stageData);
            }
        }
        public void DeleteStageData(int _stage)
        {
            if (IsConstainsStage(_stage))
            {
                stageDataDic.Remove(_stage);
            }
        }
        public void DeleteStageData(StageData _stageData)
        {
            DeleteStageData(_stageData.stage);
        }

        public StageData GetStageData(int _stage) => stageDataDic.ContainsKey(_stage) ? stageDataDic[_stage] : null;
        public TileData[,] GetTileArrByStage(int _stage) => stageDataDic.ContainsKey(_stage) ? stageDataDic[_stage].tileArr : null;
        public bool IsConstainsStage(int _stage) => stageDataDic.ContainsKey(_stage);
        public List<StageData> GetStageDataList() => stageDataDic.Values.ToList();        
        public void DataLoad()
        {
            // 일단 여기서 먼가 생성을 하긴 해야 함 데이터를
            List<StageData> stageDataList = GameUtil.LoadJsonArray<StageData>(StaticDefine.json_data_path, StaticDefine.JSON_MAP_FILE_NAME);

            stageDataDic = stageDataList.ToDictionary(k => k.stage, v => v);
        }
        public void DataSave()
        {
            GameUtil.CreateJsonFile(StaticDefine.json_data_path, StaticDefine.JSON_MAP_FILE_NAME, GetStageDataList());
        }
        public void DataSave<T>(params T[] _params) where T : GameData
        {
            List<StageData> stageList = _params.Select(x => x as StageData).ToList();

            foreach(var stage in stageList)
            {
                AddStageData(stage);
            }

            GameUtil.CreateJsonFile(StaticDefine.json_data_path, StaticDefine.JSON_MAP_FILE_NAME, GetStageDataList());
        }
    }
}

