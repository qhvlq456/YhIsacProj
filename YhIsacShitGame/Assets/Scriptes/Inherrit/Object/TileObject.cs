using UnityEngine;

namespace YhProj.Game.Map
{
    public class TileObject : MonoBehaviour
    {
        public TileData tileData;
        public virtual void Create<T>(T _data) where T : TileData
        {
            tileData = _data;
        }
        public virtual void Update()
        {
            
        }
        public virtual void Delete()
        {
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(BaseType.tile, transform);
        }
    }
}
