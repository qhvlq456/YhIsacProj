using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj;

/// <summary>
/// Map에 대한 데이터를 총괄
/// map을 create, update, delete를 한다
/// 후에 모드 컨트롤러를 생성하여 다른 모드들에 대한 클래스를 정의 후 모드에 따라 분류하여 로직을 만들어 사용하자 아마 inputmanager보면 알듯?
/// </summary>
public class MapManager : BaseManager
{
    Transform root;

    // 각 행과 열에 맞는 tile data를 가지고 있음
    // 후에 stage 를 삭제하고 dictionary로 관리할 것도 생각해 봐야 함
    Dictionary<int, StageData> stageDataDic = new Dictionary<int, StageData>();
    // end


    //  start # mode : maptool
    Dictionary<int, float> rowLinePoniterDic = new Dictionary<int, float>();
    Dictionary<int, float> colLinePoniterDic = new Dictionary<int, float>();
    List<TileObject> tileObjectList = new List<TileObject>();
    // end

    List<LineRenderer> rowLineRendererList = new List<LineRenderer>();
    List<LineRenderer> colLineRendererList = new List<LineRenderer>();
    public override void Load(Define.GameMode _gameMode)
    {
        if(root == null)
        {
            root = Util.AttachObj<Transform>("Map");
            root.transform.position = Vector3.zero;
        }

        switch (_gameMode)
        {
            case Define.GameMode.EDITOR:
                EventMediator.Instance.OnPlayerLevelChange -= OnPlayerLevelChange;
                EventMediator.Instance.OnPlayerLevelChange += OnPlayerLevelChange;
                break;
            case Define.GameMode.TEST:
                break;
            case Define.GameMode.MAPTOOL:
                // 일단 여기서 먼가 생성을 하긴 해야 함 데이터를
                List<StageData> stageDataList = Util.LoadJsonArray<StageData>(StaticDefine.JSON_MAP_DATA_PATH, StaticDefine.JSON_MAP_FILE_NAME);

                stageDataDic = stageDataList.ToDictionary(k => k.stage, v => v);

                BoxCollider bottomColider = Util.AttachObj<BoxCollider>("Bottom");
                bottomColider.size = new Vector3(100, 0, 100);

                EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
                EventMediator.Instance.OnLoadSequenceEvent += LoadPlayerEvent;
                break;
        }
    }
    // 타일에 대한 delete, update라고 생각하면 안되긴 함...
    public override void Delete()
    {
        EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
        EventMediator.Instance.OnPlayerLevelChange -= OnPlayerLevelChange;
    }

    /// <summary>
    /// stage에 맞는 데이터를 update하는 함수
    /// tileobject에 존재하는 tiledata를 업데이트 하고 렌더링을 다시 함
    /// 인포에서 데이터 set하려고 한거임
    /// </summary>
    public override void Update()
    {
     
    }

    // 말이 안되긴 함 편집이면 편집 , 생성이면 생성을 따로 구별 할 필요가 있음

    #region Tile Load and Delete and Save
    public void LoadTile(StageData _stageData)
    {
        // 있어도 덮어 쓰게끔 변경

        float z = 0f;
        float x = 0f;

        for (int i = 0; i < _stageData.Row; i++)
        {
            x = 0f;
            for (int j = 0; j < _stageData.Col; j++)
            {
                TileData tileData = new TileData();

                if (_stageData.tileArr[i, j] != null)
                {
                    tileData = _stageData.tileArr[i, j];
                }
                else
                {
                    tileData.index = i * _stageData.Row + j;
                    tileData.type = Define.BaseType.TILE;
                    tileData.roadType = Define.RoadType.ENEMY;
                }

                EditorTileObject editorTileObject = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(Define.BaseType.TILE, "EditorTileObject").GetComponent<EditorTileObject>();

                editorTileObject.transform.position = new Vector3(x, 0, z);
                editorTileObject.transform.parent = root;
                editorTileObject.Load(tileData);
                tileObjectList.Add(editorTileObject);

                x += _stageData.xOffset;
            }

            z += _stageData.zOffset;
        }
    }
    public void DeleteTile()
    {
        // object pool을 이용하여 회수 할 것임
        for (int i = 0; i < tileObjectList.Count; i++)
        {
            tileObjectList[i].Delete();
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.TILE, tileObjectList[i].transform);
            // tile object 를 초기화 object를 초기화 함으로써 data도 초기화 됨
        }
    }
    #endregion
    // player가 레벨업을 할 때마다 호출
    public void OnPlayerLevelChange(int _level)
    {
        //StageData stageData = stageDataList.Find(x => x.lv == _level);
    }
    public void LoadPlayerEvent(PlayerInfo _playerInfo)
    {
        // 이게 의존성이 인풋에 대한 의존성이 많아지니 계속 의존성이 생기게 됨 그럼으로 인풋의 target을 의존성을 없애는 작업 필요!!
        int halfIdx = StaticDefine.MAX_CREATE_TILE_NUM / 2;

        Vector3 targetPos = new Vector3(colLinePoniterDic[halfIdx], 10, rowLinePoniterDic[halfIdx]);

        // 후에 변경할것?
        Managers.Instance.lookTarget.transform.position = targetPos;
    }

    #region MapTool Mode
    [Obsolete]
    void LoadMapTool()
    {
        /*
         * x, y = 0,0 에서 양수로 시작됨
         * 렌더링을 하여 각 구역(행과 열)을 미리 그려줌
         * 일단 환경은 윈도우
         * 인풋에 대한 기획도 필요
         */

        BoxCollider bottomColider = Util.AttachObj<BoxCollider>("Bottom");
        bottomColider.size = new Vector3(100, 0, 100);

        float xOffset = 1;
        float zOffset = 1;
        // x position set 
        for (int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
        {
            float x = i * xOffset;
            colLinePoniterDic.Add(i, x);
        }

        // z position set 
        for (int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
        {
            float z = i * zOffset;
            rowLinePoniterDic.Add(i, z);
        }

        // row numbering
        for(int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
        {
            Transform rowTrf = Util.AttachObj<Transform>($"Row_Number_{i}");
            Transform colTrf = Util.AttachObj<Transform>($"Col_Number_{i}");

            rowTrf.parent = root;
            colTrf.parent = root;

            rowTrf.transform.localPosition = new Vector3(rowLinePoniterDic[i], 0, -1f);
            colTrf.transform.localPosition = new Vector3(-1f, 0, colLinePoniterDic[i]);
        }


        int idx = 0;
        for (int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
        {
            float z = rowLinePoniterDic[i];

            for (int j = 0; j < StaticDefine.MAX_CREATE_TILE_NUM; j++)
            {
                float x = colLinePoniterDic[j];
                EditorTileObject editorTileObject = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(Define.BaseType.TILE, "EditorTileObject").GetComponent<EditorTileObject>();
                editorTileObject.transform.parent = root;
                // builder든 뭐든간에 데이터를 넣어야 함
                editorTileObject.transform.localPosition = new Vector3(x, 0, z);
                editorTileObject.Load(new TileData(idx, "MapToolTile_" + idx, Define.BaseType.TILE, Define.Direction.LEFT));
                tileObjectList.Add(editorTileObject);
                idx++;
            }
        }
    }

    public void SaveMapTool(StageData _stage)
    {
        TileData[,] tileDataArr = new TileData[_stage.Row, _stage.Col];
        
        for (int i = 0; i < _stage.Row; i++)
        {
            for (int j = 0; j < _stage.Col; j++)
            {
                int idx = i * _stage.Row + j;
                TileData tileData = tileObjectList[idx].tileData;
                // builder든 뭐든간에 데이터를 넣어야 함
                tileDataArr[i, j] = tileData;
            }
        }

        _stage.tileArr = tileDataArr;
        
        if (stageDataDic.ContainsKey(_stage.stage))
        {
            stageDataDic[_stage.stage] = _stage;
        }
        else
        {
            stageDataDic.Add(_stage.stage ,_stage);
        }

        Util.CreateJsonFile(StaticDefine.JSON_MAP_DATA_PATH, "StageData", stageDataDic.Values.ToList());
    }
    #endregion

    #region Draw LineRenderer
    [Obsolete]
    void DrawLineRenderer()
    {
        Transform rowLinerendererTrf = Util.AttachObj<Transform>("rowLineParent");
        Transform colLinerendererTrf = Util.AttachObj<Transform>("colLineParent");

        for (int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
        {
            rowLineRendererList.Add(Util.AttachObj<LineRenderer>($"rowLineRenderer_{i}"));
            rowLineRendererList[i].transform.parent = rowLinerendererTrf;
            rowLineRendererList[i].startWidth = .25f;
            rowLineRendererList[i].endWidth = .25f;

            colLineRendererList.Add(Util.AttachObj<LineRenderer>($"colLineRenderer_{i}"));
            colLineRendererList[i].transform.parent = colLinerendererTrf;
            colLineRendererList[i].startWidth = .25f;
            colLineRendererList[i].endWidth = .25f;
        }

        for (int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
        {
            LineRenderer rowLineRenderer = rowLineRendererList[i];
            LineRenderer colLineRenderer = colLineRendererList[i];

            List<Vector3> rowPointerList = new List<Vector3>();
            List<Vector3> colPointerList = new List<Vector3>();

            for (int j = 0; j < StaticDefine.MAX_CREATE_TILE_NUM; j++)
            {
                float rowPointerX = rowLinePoniterDic[i];
                float rowPointerZ = colLinePoniterDic[j];

                float colPointerX = colLinePoniterDic[j];
                float colPointerZ = rowLinePoniterDic[i];

                Vector3 rowPos = new Vector3(rowPointerX, 0, rowPointerZ);
                Vector3 colPos = new Vector3(colPointerX, 0, colPointerZ);

                rowPointerList.Add(rowPos);
                colPointerList.Add(colPos);
            }

            rowLineRenderer.positionCount = rowPointerList.Count;
            rowLineRenderer.SetPositions(rowPointerList.ToArray());

            colLineRenderer.positionCount = colPointerList.Count;
            colLineRenderer.SetPositions(colPointerList.ToArray());
        }
    }

    #endregion

}
