using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using YhProj;

/// <summary>
/// Map�� ���� �����͸� �Ѱ�
/// map�� create, update, delete�� �Ѵ�
/// </summary>
public class MapManager : BaseManager
{
    // start # : mode none
    List<TilePosition> tilePositionList = new List<TilePosition>();


    // �� ��� ���� �´� tile data�� ������ ����
    List<StageData> stageDataList = new List<StageData>();
    List<TileObject> currentTileObjList = new List<TileObject>();
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
        switch(_gameMode)
        {
            case Define.GameMode.EDITOR:
                LoadDefault();
                break;
            case Define.GameMode.TEST:
                break;
            case Define.GameMode.MAPTOOL:
                LoadMapTool();
                break;
        }
    }
    #region Tile Create and Remove
    public void CreateTile(int _stage)
    {
        StageData stageData = stageDataList.Find(x => x.stage == _stage);
        TilePosition tilePosition = tilePositionList.Find(x => x.stage == _stage);

        for(int i = 0; i < stageData.tileArr.GetLength(0); i++)
        {
            for(int j = 0; j < stageData.tileArr.GetLength(1); j++)
            {
                TileData tileData = stageData.tileArr[i, j];

                TileObject tileObject = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(Define.BaseType.TILE, tileData.name).GetComponent<TileObject>();
                tileObject.transform.position = tilePosition.GetIndexByPosition(i, j);
                tileObject.Load(tileData);

                currentTileObjList.Add(tileObject);
            }
        }
    }
    public void RemoveTile()
    {
        for(int i = 0; i < currentTileObjList.Count; i++)
        {
            currentTileObjList[i].Delete();
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.TILE, currentTileObjList[i].transform, 0);
        }
    }
    #endregion
    // Ÿ�Ͽ� ���� delete, update��� �����ϸ� �ȵǱ� ��...
    public override void Delete()
    {
        // object pool�� �̿��Ͽ� ȸ�� �� ����
        for (int i = 0; i < currentTileObjList.Count; i++)
        {
            // tile object �� �ʱ�ȭ object�� �ʱ�ȭ �����ν� data�� �ʱ�ȭ ��
            currentTileObjList[i].Delete();
        }
    }

    /// <summary>
    /// stage�� �´� �����͸� update�ϴ� �Լ�
    /// tileobject�� �����ϴ� tiledata�� ������Ʈ �ϰ� �������� �ٽ� ��
    /// �������� ������ set�Ϸ��� �Ѱ���
    /// </summary>
    public override void Update()
    {
        
    }
    // player�� �������� �� ������ ȣ��
    public void OnPlayerLevelChange(int _level)
    {
        //StageData stageData = stageDataList.Find(x => x.lv == _level);
    }

    #region Default Mode
    void LoadDefault()
    {
        EventMediator.Instance.OnPlayerLevelChange -= OnPlayerLevelChange;
        EventMediator.Instance.OnPlayerLevelChange += OnPlayerLevelChange;

        // ���� �ʿ� � ��� �� �������� � ���� �ʱ�ȭ�� �Ǿ�� ��
        for (int i = 0; i < stageDataList.Count; i++)
        {
            StageData stageData = stageDataList[i];
            List<float> xPostionList = new List<float>();
            List<float> zPostionList = new List<float>();

            float x = 0;
            float z = 0;

            for (int j = 0; j < stageData.tileArr.GetLength(0); j++)
            {
                xPostionList.Add(x);
                x += stageData.xOffset;
            }

            for (int j = 0; j < stageData.tileArr.GetLength(1); j++)
            {
                zPostionList.Add(z);
                z += stageData.zOffset;
            }

            tilePositionList.Add(new TilePosition(stageData.stage, xPostionList, zPostionList));
        }

        if (stageDataList.Count > 0)
        {
            return;
        }

        List<StageData> stageDataJsonList = Util.LoadJsonArray<StageData>(StaticDefine.JSON_MAP_DATA_PATH, StaticDefine.JSON_MAP_FILE_NAME);

        if (stageDataList != null)
        {
            foreach (var stageData in stageDataList)
            {
                stageDataList.Add(stageData);
            }
        }
        else
        {
            Debug.LogError("Not Load StageData!!");
        }
    }
    #endregion

    #region MapTool Mode
    void LoadMapTool()
    {
        /*
         * x, y = 0,0 ���� ����� ���۵�
         * �������� �Ͽ� �� ����(��� ��)�� �̸� �׷���
         * �ϴ� ȯ���� ������
         * ��ǲ�� ���� ��ȹ�� �ʿ�
         */

        Transform rowLinerendererTrf = Util.AttachObj<Transform>("rowLineParent");
        Transform colLinerendererTrf = Util.AttachObj<Transform>("colLineParent");

        for (int i = 0; i < StaticDefine.MAX_CREATE_TILE_NUM; i++)
        {
            rowLineRendererList.Add(Util.AttachObj<LineRenderer>($"rowLineRenderer_{i}"));
            rowLineRendererList[i].transform.parent = rowLinerendererTrf;
            rowLineRendererList[i].startWidth = .5f;
            rowLineRendererList[i].endWidth = .5f;

            colLineRendererList.Add(Util.AttachObj<LineRenderer>($"colLineRenderer_{i}"));
            colLineRendererList[i].transform.parent = colLinerendererTrf;
            colLineRendererList[i].startWidth = .5f;
            colLineRendererList[i].endWidth = .5f;
        }

        float xOffset = 2.5f;
        float zOffset = 2.5f;

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

        TestCamera camera = Util.AttachObj<TestCamera>(Camera.main.gameObject, "Main Camera");

        int halfIdx = StaticDefine.MAX_CREATE_TILE_NUM / 2;

        Vector3 cameraPos = new Vector3(colLinePoniterDic[halfIdx], 10, rowLinePoniterDic[halfIdx]);

        camera.transform.position = cameraPos;
    }
    #endregion
}
