using UnityEngine;

namespace YhProj.Game.Map
{
    public interface ITileFactory
    {
        public TileObject Create(TileData _tileData);
    }

    public class TileFactory : ITileFactory
    {
        public TileObject Create(TileData _tileData)
        {
            // ObjectPoolManager를 사용하여 HeroObject를 생성하고 반환
            Transform trf = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(_tileData.type, _tileData.resName);
            return trf.GetComponent<TileObject>();
        }
    }

    public class EditorTileFactory : ITileFactory
    {
        public TileObject Create(TileData _tileData)
        {
            // ObjectPoolManager를 사용하여 HeroObject를 생성하고 반환
            Transform trf = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(_tileData.type, _tileData.resName);
            return trf.GetComponent<EditorTileObject>();
        }
    }
}

