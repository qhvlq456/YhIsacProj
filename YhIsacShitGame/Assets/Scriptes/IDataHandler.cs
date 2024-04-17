using System.Collections.Generic;
using System.Linq;

namespace YhProj.Game
{
    /// <summary>
    /// controller에 따라 데이터 저장 여부가 나뉨
    /// </summary>
    public interface IDataHandler<T>
    {
        void AddData(T _data);
        void DeleteData(int _index);
        T GetData(int _index);
        bool ContainsData(int _index);
        List<T> GetDataList();
        void LoadData();
        void SaveData();

        void SaveData(params T[] _params);
    }

    public abstract class BaseDataHandler<T> : IDataHandler<T> where T : GameData
    {
        protected Dictionary<int, T> dataDic = new Dictionary<int, T>();

        public virtual void AddData(T _data)
        {
            if (ContainsData(_data.index))
            {
                dataDic[_data.index] = _data;
            }
            else
            {
                dataDic.Add(_data.index, _data);
            }
        }

        public virtual void DeleteData(int _index)
        {
            if (ContainsData(_index))
            {
                dataDic.Remove(_index);
            }
        }

        public virtual T GetData(int _index)
        {
            return ContainsData(_index) ? dataDic[_index] : null;
        }

        public virtual bool ContainsData(int _index)
        {
            return dataDic.ContainsKey(_index);
        }

        public virtual List<T> GetDataList()
        {
            return dataDic.Values.ToList();
        }
        public abstract void LoadData();
        public abstract void SaveData();

        public abstract void SaveData(params T[] _params);
    }
}
