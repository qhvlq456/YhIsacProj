namespace YhProj
{
    [System.Serializable]
    public abstract class BaseData : IJson, IDataBase, ILogger
    {
        public int index { set; get; }
        public Define.BaseType type { set; get; }
        public string name { set; get; }


        public abstract void Load();
        public abstract void Update();
        public abstract void Delete();
        #region IDataBase, DBsend, DBCallback
        public virtual void DBCallback()
        {

        }

        public virtual void DBSend(params BaseData[] _parameters)
        {

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

