
using System.Diagnostics;
using UnityEditor.MemoryProfiler;

namespace YhProj.Game.Map
{
    public class TileObject : BaseObject
    {
        // 나중에 save해야 하는뎅
        public TileData tileData;
        //public Renderer renderer;

        public override void Create<T>(T _baseData)
        {
            tileData = _baseData as TileData;
        }
        public override void Update()
        {
            
        }
        public override void Delete()
        {
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.TILE, transform);
        }
    }
}
