using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj;

public class EditorMapController : MapController<TileObject>, IJson
{
    private Dictionary<int, float> rowLinePoniterDic = new Dictionary<int, float>();
    private Dictionary<int, float> colLinePoniterDic = new Dictionary<int, float>();

    private List<LineRenderer> rowLineRendererList = new List<LineRenderer>();
    private List<LineRenderer> colLineRendererList = new List<LineRenderer>();

    public EditorMapController(MapManager _manager) : base(_manager)
    {
        // 생성자 로직 추가
    }

    public override void Load()
    {
        // 에디터 모드에서의 로드 동작
        // 맵 편집 기능 등 추가
        // 일단 여기서 먼가 생성을 하긴 해야 함 데이터를


        BoxCollider bottomColider = Util.AttachObj<BoxCollider>("Bottom");
        bottomColider.size = new Vector3(100, 0, 100);
    }

    public override void Update()
    {
        // 에디터 모드에서의 업데이트 동작
    }

    public override void Delete()
    {
        // 에디터 모드에서의 삭제 동작
        // object pool을 이용하여 회수 할 것임
        for (int i = 0; i < tileList.Count; i++)
        {
            tileList[i].Delete();
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.TILE, tileList[i].transform);
            // tile object 를 초기화 object를 초기화 함으로써 data도 초기화 됨
        }
    }
    // 식 변경 필요 인덱스를 기준으로 오프셋 이동로직으로 변경
    // 아직도 문제가 존재함
    // 간헐적 랜덤으로 변경되는데 이유를 모르겠네
    public override void LoadTile(StageData _stageData)
    {
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
                    tileData.type = Define.BaseType.TILE;
                    tileData.roadType = Define.RoadType.ENEMY;
                }

                // 인덱스는 기본으로 정해져야 함
                tileData.index = i * _stageData.Col + j;

                EditorTileObject editorTileObject = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(Define.BaseType.TILE, "EditorTileObject").GetComponent<EditorTileObject>();

                editorTileObject.name = (i * _stageData.Col + j).ToString();
                editorTileObject.transform.position = new Vector3(x, 0, z);
                editorTileObject.transform.parent = mapManager.root;
                editorTileObject.Load(tileData);
                tileList.Add(editorTileObject);

                x += _stageData.xOffset;
            }

            z += _stageData.zOffset;
        }
    }
    public override void DeleteTile()
    {
        // object pool을 이용하여 회수 할 것임
        for (int i = 0; i < tileList.Count; i++)
        {
            tileList[i].Delete();
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.TILE, tileList[i].transform);
            // tile object 를 초기화 object를 초기화 함으로써 data도 초기화 됨
        }
    }

    public void SaveJson<T>(T _data)
    {
        if (_data is StageData stage)
        {
            TileData[,] tileDataArr = new TileData[stage.Row, stage.Col];

            for (int i = 0; i < stage.Row; i++)
            {
                for (int j = 0; j < stage.Col; j++)
                {
                    int idx = i * stage.Col + j;
                    TileData tileData = tileList[idx].tileData;
                    // builder든 뭐든간에 데이터를 넣어야 함
                    tileDataArr[i, j] = tileData;
                }
            }

            stage.tileArr = tileDataArr;

            if (mapManager.IsConstainsStage(stage.stage))
            {
                mapManager.GetStageData(stage.stage).tileArr = stage.tileArr;
            }
            else
            {
                mapManager.GetStageDataList().Add(stage);
            }

            Util.CreateJsonFile(StaticDefine.JSON_MAP_DATA_PATH, StaticDefine.JSON_MAP_FILE_NAME, mapManager.GetStageDataList());
        }
    }

    public void LoadJson()
    {
        throw new NotImplementedException();
    }
    #region MapTool Mode
    [Obsolete]
    void LoadMapTool(Transform _parent)
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
        for (int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
        {
            Transform rowTrf = Util.AttachObj<Transform>($"Row_Number_{i}");
            Transform colTrf = Util.AttachObj<Transform>($"Col_Number_{i}");

            rowTrf.parent = _parent;
            colTrf.parent = _parent;

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
                editorTileObject.transform.parent = _parent;
                // builder든 뭐든간에 데이터를 넣어야 함
                editorTileObject.transform.localPosition = new Vector3(x, 0, z);
                editorTileObject.Load(new TileData(idx, "MapToolTile_" + idx, Define.BaseType.TILE, Define.Direction.LEFT));
                tileList.Add(editorTileObject);
                idx++;
            }
        }
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
