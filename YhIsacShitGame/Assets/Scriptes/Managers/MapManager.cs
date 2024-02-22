using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj;

/// <summary>
/// Map�� ���� �����͸� �Ѱ�
/// map�� create, update, delete�� �Ѵ�
/// �Ŀ� ��� ��Ʈ�ѷ��� �����Ͽ� �ٸ� ���鿡 ���� Ŭ������ ���� �� ��忡 ���� �з��Ͽ� ������ ����� ������� �Ƹ� inputmanager���� �˵�?
/// </summary>
public class MapManager : BaseManager
{
    Transform root;

    // �� ��� ���� �´� tile data�� ������ ����
    // �Ŀ� stage �� �����ϰ� dictionary�� ������ �͵� ������ ���� ��
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
                // �ϴ� ���⼭ �հ� ������ �ϱ� �ؾ� �� �����͸�
                List<StageData> stageDataList = Util.LoadJsonArray<StageData>(StaticDefine.JSON_MAP_DATA_PATH, StaticDefine.JSON_MAP_FILE_NAME);

                stageDataDic = stageDataList.ToDictionary(k => k.stage, v => v);

                BoxCollider bottomColider = Util.AttachObj<BoxCollider>("Bottom");
                bottomColider.size = new Vector3(100, 0, 100);

                EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
                EventMediator.Instance.OnLoadSequenceEvent += LoadPlayerEvent;
                break;
        }
    }
    // Ÿ�Ͽ� ���� delete, update��� �����ϸ� �ȵǱ� ��...
    public override void Delete()
    {
        EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
        EventMediator.Instance.OnPlayerLevelChange -= OnPlayerLevelChange;
    }

    /// <summary>
    /// stage�� �´� �����͸� update�ϴ� �Լ�
    /// tileobject�� �����ϴ� tiledata�� ������Ʈ �ϰ� �������� �ٽ� ��
    /// �������� ������ set�Ϸ��� �Ѱ���
    /// </summary>
    public override void Update()
    {
     
    }

    // ���� �ȵǱ� �� �����̸� ���� , �����̸� ������ ���� ���� �� �ʿ䰡 ����

    #region Tile Load and Delete and Save
    public void LoadTile(StageData _stageData)
    {
        // �־ ���� ���Բ� ����

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
        // object pool�� �̿��Ͽ� ȸ�� �� ����
        for (int i = 0; i < tileObjectList.Count; i++)
        {
            tileObjectList[i].Delete();
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.TILE, tileObjectList[i].transform);
            // tile object �� �ʱ�ȭ object�� �ʱ�ȭ �����ν� data�� �ʱ�ȭ ��
        }
    }
    #endregion
    // player�� �������� �� ������ ȣ��
    public void OnPlayerLevelChange(int _level)
    {
        //StageData stageData = stageDataList.Find(x => x.lv == _level);
    }
    public void LoadPlayerEvent(PlayerInfo _playerInfo)
    {
        // �̰� �������� ��ǲ�� ���� �������� �������� ��� �������� ����� �� �׷����� ��ǲ�� target�� �������� ���ִ� �۾� �ʿ�!!
        int halfIdx = StaticDefine.MAX_CREATE_TILE_NUM / 2;

        Vector3 targetPos = new Vector3(colLinePoniterDic[halfIdx], 10, rowLinePoniterDic[halfIdx]);

        // �Ŀ� �����Ұ�?
        Managers.Instance.lookTarget.transform.position = targetPos;
    }

    #region MapTool Mode
    [Obsolete]
    void LoadMapTool()
    {
        /*
         * x, y = 0,0 ���� ����� ���۵�
         * �������� �Ͽ� �� ����(��� ��)�� �̸� �׷���
         * �ϴ� ȯ���� ������
         * ��ǲ�� ���� ��ȹ�� �ʿ�
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
                // builder�� ���簣�� �����͸� �־�� ��
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
                // builder�� ���簣�� �����͸� �־�� ��
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
