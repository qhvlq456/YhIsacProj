using YhProj;


public class TileObject : BaseObject
{
    // 나중에 save해야 하는뎅
    public TileData tileData;
    //public Renderer renderer;

    // 흠 이것도 object로 만들어서 다음 tile obj, etcobj 등등 나눈것도 괜찮을 것 같음 
    // dbsend, callback까지 다 받은 데이터라고 생각하고 작성

    public override void Load<T>(T _baseData)
    {
        tileData = _baseData as TileData;
    }
    public override void Delete()
    {
        Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.TILE, transform);
        tileData.Delete();
    }
}
