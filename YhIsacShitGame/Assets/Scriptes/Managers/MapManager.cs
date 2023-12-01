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
    List<TilePosition> tilePositionList = new List<TilePosition>();


    // �� ��� ���� �´� tile data�� ������ ����
    List<StageData> stageDataList = new List<StageData>();
    List<TileObject> currentTileObjList = new List<TileObject>();

    public override void Load()
    {
        EventMediator.Instance.OnPlayerLevelChange -= OnPlayerLevelChange;
        EventMediator.Instance.OnPlayerLevelChange += OnPlayerLevelChange;

        for (int i = 0; i < stageDataList.Count; i++)
        {
            StageData stageData = stageDataList[i];
            List<float> xPostionList = new List<float>();
            List<float> zPostionList = new List<float>();

            float x = 0;
            float z = 0;

            for(int j = 0; j < stageData.tileArr.GetLength(0); j++)
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

}
