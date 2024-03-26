namespace YhProj.Game
{
    [System.Serializable]
    public abstract class BaseData
    {
        public int index;
        public Define.BaseType type;
        public string name;
        public virtual void Load<T>()
        { 

        }

        public abstract void Update(BaseData _baseData);
        public abstract void Delete();
    }
}

