using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using YhProj.Game.Character;
using YhProj.Game.Play;
using YhProj.Game.Player;

namespace YhProj.Game.Map
{
    // 함수를 인터페이스로 구현하여 역전의존성 하면 객체지향적으로 변하는구나...


    /// <summary>
    /// Map에 대한 데이터를 총괄
    /// map을 create, update, delete를 한다
    /// 후에 모드 컨트롤러를 생성하여 다른 모드들에 대한 클래스를 정의 후 모드에 따라 분류하여 로직을 만들어 사용하자 아마 inputmanager보면 알듯?
    /// </summary>
    /// 여기서 tile과 stage 두개를 분리하여 관리
    public class MapManager : BaseManager, IGameFlow, IBuildable
    {
        // 설치 가능한 grid 표시 작업 필요 // 행과 열 구분이 필요함... 하지 않을지도? 그냥 batchidx == 0인것만 찾음 됨
        // input에서 관리해야 할 것 같은데?
        // 여기서 combine해서 결과물 도출하여야 함

        // navmesh를 구워야 함
        private TileDataHandler tileDataHandler;
        private List<TileObject> instanceTileList = new List<TileObject>();
        public List<TileObject> InstanceTileList => instanceTileList;
        

        private ITileFactory tileFactory;

        public void OnStart()
        {
            StageData stageData = GameManager.Instance.stageData;

            string log = "";

            List<int> list = stageData.tileIdxList;

            int tileCount = stageData.row * stageData.col;

            for (int i = 0; i < tileCount; i++)
            {
                // 이거 생각해봐야 함
                int z = i / stageData.row;
                int x = i % stageData.col;

                // 생성하는 부분이 사라졌네??
                TileObject tileObject = null;
                TileData tileData = tileDataHandler.GetData(list[i]);

                tileObject.transform.SetParent(root);
                tileObject.transform.localPosition = new Vector3(x, 0, z);
                instanceTileList.Add(tileObject);
                log += $"idx = {tileData.index}, road type = {tileData.elementType}, \n";
            }

            // 후에 navmesh굽는 작업 필요
            Debug.LogError(log);
        }

        public void OnUpdate()
        {
            
        }

        public void OnEnd()
        {
            foreach (var item in instanceTileList)
            {
                item?.Delete();
            }
        }

        public override void Load()
        {
            if (root == null)
            {
                root = GameUtil.AttachObj<Transform>("MapRoot");
                root.transform.position = Vector3.zero;
            }

            // 나중에 각 매니저마다 비동기식으로 로드 할 것
            tileDataHandler.LoadJsonData();
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


        #region Grid
        public bool IsBuildable(Vector3Int _startPosition, Vector2Int _size)
        {
            // 예시: position에 빌드 가능한지 판단하는 로직
            foreach (var tile in instanceTileList)
            {

            }

            return false;
        }
        #endregion
    }
    public sealed class TileDataHandler : BaseDataHandler<TileData>
    {
        public static string json_tile_file_name = "StageData.json";
        public override void LoadJsonData()
        {
            List<TileData> list = GameUtil.LoadJsonArray<TileData>(StaticDefine.json_data_path, json_tile_file_name);

            foreach(var data in list) 
            { 
                if(!dataMap.ContainsKey(data.index)) 
                {
                    dataMap.Add(data.index, data);
                }
                else
                {
                    Debug.LogError($"[TileHandler] Already is constains key : {data.index}!!");
                }
            }
        }

        public override void SaveJsonData()
        {
            // 각 tiledata 클래스 별로 나누어 저장을 해야 할지 아님 한번에 tiledata로 저장해야 할지 알아보긴 해야 함
            // GameUtil.CreateJsonFile(StaticDefine.json_data_path, JSON_MAP_FILE_NAME, GetDataList());
        }


        public override void SaveJsonData(params JsonReceiveDataArgs[] _params)
        {
            // 각 tiledata 클래스 별로 나누어 저장을 해야 할지 아님 한번에 tiledata로 저장해야 할지 알아보긴 해야 함

            List<TileData> tileDataList = _params.OfType<TileData>().ToList();

            foreach (var tileData in tileDataList)
            {
                AddData(tileData.index, tileData);
            }

            // GameUtil.CreateJsonFile(StaticDefine.json_data_path, StaticDefine.JSON_MAP_FILE_NAME, GetDataList());
        }
    }
}

