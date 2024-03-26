namespace YhProj.Game
{
    public interface IDataBase
    {
        void DBSend(params BaseData[] _parameters);
        void DBCallback(params BaseData[] _parameters);
    }
}
