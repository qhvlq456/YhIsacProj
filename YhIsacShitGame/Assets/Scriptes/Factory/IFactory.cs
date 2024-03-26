namespace YhProj.Game
{
    public interface IFactory
    {
        V Create<T, V>(T _data)
            where T : BaseData
            where V : BaseObject;
    }
}

