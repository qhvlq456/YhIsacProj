using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj;

// ijson으로 이것들도 load해야 함
/// <summary>
/// Map에 대한 데이터를 총괄
/// map을 create, update, delete를 한다
/// 후에 모드 컨트롤러를 생성하여 다른 모드들에 대한 클래스를 정의 후 모드에 따라 분류하여 로직을 만들어 사용하자 아마 inputmanager보면 알듯?
/// </summary>
public class MapManager : BaseManager
{
    public Transform root { get; private set; }

    private MapController mapController;

    private IJson json;

    private Dictionary<int, StageData> stageDataDic = new Dictionary<int, StageData>();
    public void AddStageData(StageData _stageData)
    {
        if(IsConstainsStage(_stageData.stage))
        {
            stageDataDic[_stageData.stage] = _stageData;
        }
        else
        {
            stageDataDic.Add(_stageData.stage, _stageData);
        }
    }
    public StageData GetStageData(int _stage) => stageDataDic.ContainsKey(_stage) ? stageDataDic[_stage] : null;
    public TileData[,] GetTileArrByStage(int _stage) => stageDataDic.ContainsKey(_stage) ? stageDataDic[_stage].tileArr : null;
    public bool IsConstainsStage(int _stage) => stageDataDic.ContainsKey(_stage);
    public List<StageData> GetStageDataList() => stageDataDic.Values.ToList();
    

    public override void Load(Define.GameMode _gameMode)
    {
        if(root == null)
        {
            root = Util.AttachObj<Transform>("Map");
            root.transform.position = Vector3.zero;
        }

        // 일단 여기서 먼가 생성을 하긴 해야 함 데이터를
        List<StageData> stageDataList = Util.LoadJsonArray<StageData>(StaticDefine.JSON_MAP_DATA_PATH, StaticDefine.JSON_MAP_FILE_NAME);

        stageDataDic = stageDataList.ToDictionary(k => k.stage, v => v);

        switch(_gameMode)
        {
            case Define.GameMode.MAPTOOL:
                mapController = new EditorMapController(this);
                json = mapController as IJson;
                break;
        }

        mapController.Load();
    }
    // 타일에 대한 delete, update라고 생각하면 안되긴 함...
    public override void Delete()
    {
        EventMediator.OnLoadSequenceEvent -= LoadPlayerEvent;
        EventMediator.OnPlayerLevelChange -= OnPlayerLevelChange;

        mapController.Delete();
    }

    /// <summary>
    /// stage에 맞는 데이터를 update하는 함수
    /// tileobject에 존재하는 tiledata를 업데이트 하고 렌더링을 다시 함
    /// 인포에서 데이터 set하려고 한거임
    /// </summary>
    public override void Update()
    {
        mapController.Update();
    }

    // 말이 안되긴 함 편집이면 편집 , 생성이면 생성을 따로 구별 할 필요가 있음

    #region Tile Load and Delete and Save
    public void LoadTile(StageData _stageData)
    {
        mapController.LoadTile(_stageData);
    }
    public void DeleteTile()
    {
        mapController.DeleteTile();
    }
    #endregion
    
    public void SaveMapJson<T>(T _data)
    {
        // controller에 따라 변경하기 매개변수 변경
        json.SaveJson(_data);

    }

    public void DeleteMapTool(int _stage)
    {
        if(IsConstainsStage(_stage))
        {
            stageDataDic.Remove(_stage);
            Util.CreateJsonFile(StaticDefine.JSON_MAP_DATA_PATH, "StageData", stageDataDic.Values.ToList());
        }
        else
        {

        }
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
