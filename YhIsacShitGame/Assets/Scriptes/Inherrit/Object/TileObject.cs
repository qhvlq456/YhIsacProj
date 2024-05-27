namespace YhProj.Game.Map
{
    public class TileObject : BaseObject
    {
        public override void Create<T>(T _baseData)
        {
            gameData = _baseData as TileData;
        }
        public override void Update()
        {
            
        }
        public override void Delete()
        {
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(BaseType.tile, transform);
        }
    }
}
