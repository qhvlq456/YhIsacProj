using System;
using System.Collections.Generic;
using System.Linq;
using YhProj.Game.User;

namespace YhProj.Game
{
    // 후에 수정
    public class JsonReceiveDataArgs
    {

    }
    public interface IDataHandler
    {
        void LoadJsonData();
        void SaveJsonData(params JsonReceiveDataArgs[] _params);
    }
    // 서버 통신을 위한 인터페이스
    public interface IServerDataHandler
    {
        void DBRecive(params ServerReceiveEventArgs[] _parameteres);
        void DBCallback(object _sender, ServerCallbackEventArgs _args);
    }
    /// <summary>
    /// Data들 로드,저장 여부를 컨트롤 함
    /// </summary>
    public abstract class BaseDataHandler<T> : IDataHandler, IServerDataHandler  where T : class
    {
        // key : index, value : gamedata
        protected Dictionary<int, T> dataMap = new Dictionary<int, T>();
        public BaseDataHandler()
        {
            ServerCommunicator.OnServerCallback += DBCallback;
        }
        public virtual void AddData(int idx, T _data)
        {
            if (ContainsData(idx))
            {
                dataMap[idx] = _data;
            }
            else
            {
                dataMap.Add(idx, _data);
            }
        }

        public virtual void DeleteData(int _index)
        {
            if (ContainsData(_index))
            {
                dataMap.Remove(_index);
            }
        }

        public virtual T GetData(int _index)
        {
            return ContainsData(_index) ? dataMap[_index] : null;
        }

        public virtual bool ContainsData(int _index)
        {
            return dataMap.ContainsKey(_index);
        }

        public virtual List<T> GetDataList()
        {
            return dataMap.Values.OfType<T>().ToList();
        }
        public abstract void LoadJsonData();
        public abstract void SaveJsonData();

        public abstract void SaveJsonData(params JsonReceiveDataArgs[] _params);

        public virtual void DBRecive(params ServerReceiveEventArgs[] _parameteres)
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
