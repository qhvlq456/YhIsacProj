using YhProj.Game.Map;

namespace YhProj.Game
{
    /// <summary>
    /// controller에 따라 데이터 저장 여부가 나뉨
    /// </summary>
    public interface IDataHandler
    {
        void DataLoad();
        void DataSave<T>(params T[] _params) where T : BaseData;
    }
}
