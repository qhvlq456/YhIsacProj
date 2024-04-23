using System;
using System.Collections.Generic;
using System.Linq;

namespace YhProj.Game
{
    public interface IDataHandler
    {
        void LoadData();
        void SaveData();
        void SaveData<T>(params T[] _params) where T : GameData;
    }
    /// <summary>
    /// controller에 따라 데이터 저장 여부가 나뉨
    /// </summary>
    public abstract class BaseDataHandler : IDataHandler
    {
        // key : index, value : gamedata
        protected Dictionary<int, GameData> dataDic = new Dictionary<int, GameData>();
        public virtual void AddData<T>(T _data) where T : GameData
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

        public virtual T GetData<T>(int _index) where T : GameData
        {
            return ContainsData(_index) ? dataDic[_index] as T: null;
        }

        public virtual bool ContainsData(int _index)
        {
            return dataDic.ContainsKey(_index);
        }

        public virtual List<T> GetDataList<T>() where T : GameData
        {
            return dataDic.Values.OfType<T>().ToList();
        }
        public abstract void LoadData();
        public abstract void SaveData();

        public abstract void SaveData<T>(params T[] _params) where T : GameData;
    }
}





