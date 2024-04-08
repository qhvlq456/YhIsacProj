using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Map
{
    // 기본적인 map에대한 게임 구현
    public abstract class MapController<T> : IController where T : TileObject
    {
        protected MapManager manager;

        protected List<T> tileObjectList = new List<T>();
        protected IFactory factory;

        // 합쳐서 tileobjectlist에 넣어서 생성하면 됨
        public MapController(MapManager _manager, IFactory _factory)
        {
            manager = _manager;
            factory = _factory;

            Initialize();
        }
        public virtual void Initialize()
        {
            
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
        public abstract T LoadTile(TileData _tileData);
        public virtual void DeleteTile(TileData _tileData)
        {
            // 안돼면 다른 방법? 인덱스 같은걸로 변경
            TileObject tileObject = tileObjectList.Find(x => x.tileData == _tileData);
            tileObject.Delete();
        }
    }
    public sealed class HeroTileController : MapController<HeroTileObject>
    {
        public HeroTileController(MapManager _manager, IFactory _factory) : base(_manager, _factory) { }

        public override void DeleteTile(TileData _tileData)
        {
            base.DeleteTile(_tileData);
        }

        public override HeroTileObject LoadTile(TileData _tileData)
        {
            HeroTileObject heroTileObj = factory.Create<HeroTileObject>(_tileData);
            tileObjectList.Add(heroTileObj);

            return heroTileObj;
        }
    }
    public sealed class EnemyTileController : MapController<EnemyTileObject>
    {
        public EnemyTileController(MapManager _manager, IFactory _factory) : base(_manager, _factory) { }
        public override void DeleteTile(TileData _tileData)
        {
            base.DeleteTile(_tileData);
        }

        public override EnemyTileObject LoadTile(TileData _tileData)
        {
            EnemyTileObject enemyTileObj = factory.Create<EnemyTileObject>(_tileData);
            tileObjectList.Add(enemyTileObj);

            return enemyTileObj;
        }
    }
    public sealed class DecoTileController : MapController<DecoTileObject>
    {
        public DecoTileController(MapManager _manager, IFactory _factory) : base(_manager, _factory) { }
        public override void DeleteTile(TileData _tileData)
        {
            base.DeleteTile(_tileData);
        }

        public override DecoTileObject LoadTile(TileData _tileData)
        {
            DecoTileObject decoTileObj = factory.Create<DecoTileObject>(_tileData);
            tileObjectList.Add(decoTileObj);

            return decoTileObj;
        }
    }

    public sealed class EditorTileController : MapController<EditorTileObject>
    {
        public EditorTileController(MapManager _manager, IFactory _factory) : base(_manager, _factory) { }

        public override void DeleteTile(TileData _tileData)
        {
            base.DeleteTile(_tileData);
        }

        public override EditorTileObject LoadTile(TileData _tileData)
        {
            EditorTileObject editorTileObject = factory.Create<EditorTileObject>(_tileData);
            tileObjectList.Add(editorTileObject);

            return editorTileObject;
        }
    }
}
