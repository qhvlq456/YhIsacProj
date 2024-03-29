using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Map
{
    // 기본적인 map에대한 게임 구현
    public class MapController : IMapController
    {
        protected MapManager manager;
        protected List<TileObject> tileObjectList = new List<TileObject>();
        protected IFactory factory;
        public List<TileObject> TileObjectList => tileObjectList;

        public MapController(MapManager _manager)
        {
            manager = _manager;
        }
        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }
        public void Update() { }
        public void Dispose()
        {
            // object pool을 이용하여 회수 할 것임
            for (int i = 0; i < tileObjectList.Count; i++)
            {
                tileObjectList[i].Delete();
                // tile object 를 초기화 object를 초기화 함으로써 data도 초기화 됨
            }

            tileObjectList.Clear();
        }
        public virtual void LoadTile(StageData _stageData)
        {
            // 기본적인 게임 로직
            throw new NotImplementedException();
        }
        public virtual void DeleteTile(StageData _stageData)
        {
            // 기본적인 게임 로직
            throw new NotImplementedException();
        }
    }
    public sealed class EditorMapController : MapController
    {
        public EditorMapController(MapManager _manager) : base(_manager) { }
        public override void Initialize()
        {
            BoxCollider bottomColider = GameUtil.AttachObj<BoxCollider>("Bottom");
            bottomColider.size = new Vector3(100, 0, 100);
        }
        
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
                    editorTileObject.Create(tileData);
                    tileObjectList.Add(editorTileObject);
                    log += $"idx = {tileData.index}, road type = {tileData.elementType}, ";
                }
                log += '\n';
            }
            Debug.LogError(log);
        }
        public override void DeleteTile(StageData _stageData)
        {
            // 일단 이거긴 한데.. 나중에 추가 작업 필요 할 수 있음
            Dispose();
        }
    }
}
