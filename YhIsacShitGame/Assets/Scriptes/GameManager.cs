using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YhProj.Game.Map;
using YhProj.Game.Character;
using System.Linq;

namespace YhProj.Game.Play
{
    /// <summary>
    /// 게임 플레이에 있어 최고 매니저
    /// </summary>
    // GamePlayPanelUI
    public class GameManager : Singleton<GameManager>
    {
        public StageHandler stageHandler;
        // 플레이어가 선택한 스테이지 데이터
        public StageData stageData;
        private List<IGameFlow> gameFlowList = new List<IGameFlow>();

        //public Vector3 StartPosition
        //{
        //    get => 
        //}
        //public Vector3 EndPosition
        //{
        //    get =>
        //}
        protected override void Awake()
        {
            stageHandler = new StageHandler();
            stageHandler.LoadJsonData();
        }

        #region GameFlow
        public void LoadGame(int _stage)
        {
            stageData = stageHandler.GetData<StageData>(_stage);
            gameFlowList = Managers.Instance.GameFlowManagerList;
        }
        public void StartGame()
        {
            foreach(var gameFlow in gameFlowList) 
            {
                gameFlow.OnStart();
            }
        }

        public void UpdateGame()
        {
            foreach (var gameFlow in gameFlowList)
            {
                gameFlow.OnUpdate();
            }
        }

        public void EndGame()
        {
            foreach (var gameFlow in gameFlowList)
            {
                gameFlow.OnEnd();
            }
        }
        public void DisposeGame()
        {
            
        }
        #endregion



    }


    // 이게 mapmanager에 있어야 할 이유가 없는데 제일 root인데 ;;
    // Data류 클래스들은 idatahandler를 통해 재구현하고 클래스로 데이터 보관
    public sealed class StageHandler : BaseDataHandler
    {
        public static string json_stage_file_name = "StageData.json";

        public override void LoadJsonData()
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

        public override void SaveJsonData()
        {
            GameUtil.CreateJsonFile(StaticDefine.json_data_path, json_stage_file_name, GetDataList<StageData>());
        }

        public override void SaveJsonData<T>(params T[] _params)
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
