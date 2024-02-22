namespace YhProj
{
    [System.Serializable]
    public abstract class BaseData : IJson, IDataBase, ILogger
    {
        public int index;
        public Define.BaseType type;
        public string name;
        public BaseData() { }
        public BaseData(int _index, Define.BaseType _type, string _name)
        {
            index = _index;
            type = _type;
            name = _name;
        }

        public bool IsSame(int _idx)
        {
            return index == _idx;
        }
        public abstract void Load();
        public abstract void Update();
        public abstract void Delete();
        #region IDataBase, DBsend, DBCallback
        public virtual void DBCallback(params BaseData[] _parameters)
        {
            // callback attach is play event 
            // 어떤 주체에게 받은 send에 대한 callback을 받음 됨
        }

        public virtual void DBSend(params BaseData[] _parameters)
        {
            // 어떤 주체에게 send를 보내면 됨
            // set event -> send -> call -> complete

        }
        #endregion
        #region IJson JsonToData, DataToJson
        public virtual void JsonToData(string _json)
        {

        }
        public virtual void DataToJson()
        {

        }
        #endregion
        public virtual void Logger()
        {
            UnityEngine.Debug.LogWarningFormat("Base Data \n index : {0}, type : {1}", index, type);
        }
    }
}

