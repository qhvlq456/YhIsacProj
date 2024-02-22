using YhProj;


public class TileObject : BaseObject
{
    // ���߿� save�ؾ� �ϴµ�
    public TileData tileData;
    //public Renderer renderer;

    // �� �̰͵� object�� ���� ���� tile obj, etcobj ��� �����͵� ������ �� ���� 
    // dbsend, callback���� �� ���� �����Ͷ�� �����ϰ� �ۼ�

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
