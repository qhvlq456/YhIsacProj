using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Map
{
    public class MapController : BaseController<MapManager>
    {
        public virtual void LoadTile(StageData _stageData)
        {

        }
        public virtual void DeleteTile()
        {

        }
    }
    public class MapController<T> : MapController where T : TileObject
    {
        protected List<T> objectList = new List<T>();
        public MapController(BaseManager _mapManager)
        {
            manager = _mapManager as MapManager;
        }
        

    }

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


            BoxCollider bottomColider = GameUtil.AttachObj<BoxCollider>("Bottom");
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
            DeleteTile();
        }
        // 인덱스는 그냥... 흠 순서 문제인거 같은데
        public override void LoadTile(StageData _stageData)
        {
            string log = "";

            for (int i = 0; i < _stageData.Row; i++)
            {
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
                        tileData.elementType = Define.ElementType.ENEMY;
                    }

                    tileData.index = i * _stageData.Col + j;

                    EditorTileObject editorTileObject = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(Define.BaseType.TILE, "EditorTileObject").GetComponent<EditorTileObject>();
                    int z = tileData.index / _stageData.Row;
                    int x = tileData.index % _stageData.Col;

                    editorTileObject.transform.position = new Vector3(x, 0, z);
                    editorTileObject.transform.parent = manager.root;
                    editorTileObject.Load(tileData);
                    objectList.Add(editorTileObject);
                    log += $"idx = {tileData.index}, road type = {tileData.elementType}, ";
                }
                log += '\n';
            }

            Debug.LogError(log);
        }
        public override void DeleteTile()
        {
            // object pool을 이용하여 회수 할 것임
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].Delete();
                Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.TILE, objectList[i].transform);
                // tile object 를 초기화 object를 초기화 함으로써 data도 초기화 됨
            }

            objectList.Clear();
        }

        public void SaveJson<T>(T _data)
        {
            if (_data is StageData stageData)
            {
                string log = "";

                TileData[,] tileDataArr = new TileData[stageData.Row, stageData.Col];

                for (int i = 0; i < stageData.Row; i++)
                {
                    for (int j = 0; j < stageData.Col; j++)
                    {
                        int idx = i * stageData.Col + j;
                        TileData tileData = objectList[idx].tileData;
                        // builder든 뭐든간에 데이터를 넣어야 함
                        tileDataArr[i, j] = tileData;
                        log += $"idx = {tileData.index}, road type = {tileData.elementType}, ";
                    }
                    log += '\n';
                }

                stageData.tileArr = tileDataArr;

                manager.AddStageData(stageData);
                Debug.LogError(log);

                GameUtil.CreateJsonFile(StaticDefine.JSON_MAP_DATA_PATH, StaticDefine.JSON_MAP_FILE_NAME, manager.GetStageDataList());
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

            BoxCollider bottomColider = GameUtil.AttachObj<BoxCollider>("Bottom");
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
                Transform rowTrf = GameUtil.AttachObj<Transform>($"Row_Number_{i}");
                Transform colTrf = GameUtil.AttachObj<Transform>($"Col_Number_{i}");

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
                    // new TileData(idx, "MapToolTile_" + idx, Define.BaseType.TILE, Define.Direction.LEFT)
                    TileData newTileData = new TileDataBuilder().SetRoadType(Define.ElementType.MINE)
                        .SetDirection(Define.Direction.LEFT)
                        .SetBatchIdx(0)
                        .SetName("MapToolTile_" + idx)
                        .SetType(Define.BaseType.TILE)
                        .SetIndex(idx)
                        .Build();
                    editorTileObject.Load(newTileData);
                    objectList.Add(editorTileObject);
                    idx++;
                }
            }
        }
        #endregion
        #region Draw LineRenderer
        [Obsolete]
        void DrawLineRenderer()
        {
            Transform rowLinerendererTrf = GameUtil.AttachObj<Transform>("rowLineParent");
            Transform colLinerendererTrf = GameUtil.AttachObj<Transform>("colLineParent");

            for (int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
            {
                rowLineRendererList.Add(GameUtil.AttachObj<LineRenderer>($"rowLineRenderer_{i}"));
                rowLineRendererList[i].transform.parent = rowLinerendererTrf;
                rowLineRendererList[i].startWidth = .25f;
                rowLineRendererList[i].endWidth = .25f;

                colLineRendererList.Add(GameUtil.AttachObj<LineRenderer>($"colLineRenderer_{i}"));
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
}
