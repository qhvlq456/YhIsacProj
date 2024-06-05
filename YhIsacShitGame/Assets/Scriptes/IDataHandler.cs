using System;
using System.Collections.Generic;
using System.Linq;
using YhProj.Game.User;

namespace YhProj.Game
{
    public struct Tests
    {
        public int i;
        public string s;
    }
    public interface IDataHandler
    {
        void LoadJsonData();
        void SaveJsonData();
        void SaveJsonData<T>(params T[] _params) where T : GameData;
    }
    // 서버 통신을 위한 인터페이스
    public interface IServerDataHandler
    {
        // void DBSend();
        void DBSend<T>(params T[] _parameteres) where T : GameData;
        // void DBCallback<T>(params T[] _params) where T : GameData;
        void DBCallback(object _sender, ServerCallbackEventArgs _args);
    }
    /// <summary>
    /// Data들 로드,저장 여부를 컨트롤 함
    /// </summary>
    public abstract class BaseDataHandler : IDataHandler, IServerDataHandler
    {
        // key : index, value : gamedata
        protected Dictionary<int, GameData> dataMap = new Dictionary<int, GameData>();
        public BaseDataHandler()
        {
            ServerCommunicator.OnServerCallback += DBCallback;
        }
        public virtual void AddData<T>(T _data) where T : GameData
        {
            if (ContainsData(_data.index))
            {
                dataMap[_data.index] = _data;
            }
            else
            {
                dataMap.Add(_data.index, _data);
            }
        }
        public T Test<T>() where T : struct
        {
            return default(T);
        }

        public virtual void DeleteData(int _index)
        {
            if (ContainsData(_index))
            {
                dataMap.Remove(_index);
            }
        }

        public virtual T GetData<T>(int _index) where T : GameData
        {
            return ContainsData(_index) ? dataMap[_index] as T: null;
        }

        public virtual bool ContainsData(int _index)
        {
            return dataMap.ContainsKey(_index);
        }

        public virtual List<T> GetDataList<T>() where T : GameData
        {
            return dataMap.Values.OfType<T>().ToList();
        }
        public abstract void LoadJsonData();
        public abstract void SaveJsonData();

        public abstract void SaveJsonData<T>(params T[] _params) where T : GameData;

        public virtual void DBSend<T>(params T[] _parameteres) where T : GameData
        {
            ServerCommunicator.SendData(_parameteres);
        }
        public virtual void DBCallback(object _sender, ServerCallbackEventArgs _args)
        {
            //if (_params.Length > 0 && _params[0] is string response)
            //{
            //    // ... (서버 응답 데이터 처리)
            //}
        }
    }
}
