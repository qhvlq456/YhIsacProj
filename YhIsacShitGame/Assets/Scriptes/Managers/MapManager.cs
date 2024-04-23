using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using YhProj.Game.Player;

namespace YhProj.Game.Map
{
    /// <summary>
    /// Map에 대한 데이터를 총괄
    /// map을 create, update, delete를 한다
    /// 후에 모드 컨트롤러를 생성하여 다른 모드들에 대한 클래스를 정의 후 모드에 따라 분류하여 로직을 만들어 사용하자 아마 inputmanager보면 알듯?
    /// </summary>
    /// 여기서 tile과 stage 두개를 분리하여 관리
    public class MapManager : BaseManager
    {
        // 여기서 combine해서 결과물 도출하여야 함
        //public void AddStageData(StageData _stageData) => stageHandler.AddStageData(_stageData);
        //public StageData GetStageData(int _stage) => stageHandler.GetStageData(_stage);
        //public List<int> GetTileArrByStage(int _stage) => stageHandler.GetTileArrByStage(_stage);
        //public bool IsConstainsStage(int _stage) => stageHandler.IsConstainsStage(_stage);
        //public List<StageData> GetStageDataList() => stageHandler.GetStageDataList();
        public override void Load()
        {
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
    public sealed class TileHandler : BaseDataHandler
    {
        public static string json_hero_tile_file_name = "StageData.json";
        public static string json_enemy_tile_file_name = "StageData.json";
        public static string json_deco_tile_file_name = "StageData.json";
        public override void LoadData()
        {
            List<TileData> list = new List<TileData>();

            List<HeroTileData> heroTileDataList = GameUtil.LoadJsonArray<HeroTileData>(StaticDefine.json_data_path, json_hero_tile_file_name);
            List<EnemyTileData> enemyTileDataList = GameUtil.LoadJsonArray<EnemyTileData>(StaticDefine.json_data_path, json_enemy_tile_file_name);
            List<DecoTileData> decoTileDataList = GameUtil.LoadJsonArray<DecoTileData>(StaticDefine.json_data_path, json_deco_tile_file_name);

            list.AddRange(heroTileDataList);
            list.AddRange(enemyTileDataList);
            list.AddRange(decoTileDataList);

            foreach(var data in list) 
            { 
                if(!dataDic.ContainsKey(data.index)) 
                {
                    dataDic.Add(data.index, data);
                }
                else
                {
                    Debug.LogError($"[TileHandler] Already is constains key : {data.index}!!");
                }
            }
        }

        public override void SaveData()
        {
            // 각 tiledata 클래스 별로 나누어 저장을 해야 할지 아님 한번에 tiledata로 저장해야 할지 알아보긴 해야 함
            // GameUtil.CreateJsonFile(StaticDefine.json_data_path, JSON_MAP_FILE_NAME, GetDataList());
        }


        public override void SaveData<T>(params T[] _params)
        {
            // 각 tiledata 클래스 별로 나누어 저장을 해야 할지 아님 한번에 tiledata로 저장해야 할지 알아보긴 해야 함

            List<TileData> tileDataList = _params.OfType<TileData>().ToList();

            foreach (var tileData in tileDataList)
            {
                AddData(tileData);
            }

            // GameUtil.CreateJsonFile(StaticDefine.json_data_path, StaticDefine.JSON_MAP_FILE_NAME, GetDataList());
        }
    }

    // Data류 클래스들은 idatahandler를 통해 재구현하고 클래스로 데이터 보관
    public sealed class StageHandler : BaseDataHandler
    {
        public static string json_stage_file_name = "StageData.json";

        public override void LoadData()
        {
            // 일단 여기서 먼가 생성을 하긴 해야 함 데이터를
            List<StageData> stageDataList = GameUtil.LoadJsonArray<StageData>(StaticDefine.json_data_path, json_stage_file_name);


            foreach (var data in stageDataList)
            {
                if (!dataDic.ContainsKey(data.index))
                {
                    dataDic.Add(data.index, data);
                }
                else
                {
                    Debug.LogError($"[StageHandler] Already is constains key : {data.index}!!");
                }
            }
        }

        public override void SaveData()
        {
            GameUtil.CreateJsonFile(StaticDefine.json_data_path, json_stage_file_name, GetDataList<StageData>());
        }

        public override void SaveData<T>(params T[] _params)
        {
            List<StageData> stageList = _params.OfType<StageData>().ToList();

            foreach (var stage in stageList)
            {
                AddData(stage);
            }

            GameUtil.CreateJsonFile(StaticDefine.json_data_path, json_stage_file_name, GetDataList<StageData>());
        }
    }

}

