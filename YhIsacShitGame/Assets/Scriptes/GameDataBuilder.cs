namespace YhProj.Game
{
    public class GameDataBuilder<T> : IBuilder<T> where T : GameData, new ()
    {
        protected T data;

        public GameDataBuilder()
        {
            data = new T();
        }

        public GameDataBuilder<T> SetIndex(int _idx)
        {
            data.index = _idx;
            return this;
        }
        public GameDataBuilder<T> SetType(BaseType _type)
        {
            data.type = _type;
            return this;
        }
        public GameDataBuilder<T> SetType(object _type)
        {
            if (int.TryParse(_type.ToString(), out int type))
            {
                data.type = (BaseType)type;
            }

            return this;
        }

        public GameDataBuilder<T> SetName(string _name)
        {
            data.name = _name;
            return this;
        }

        public virtual T Build()
        {
            return data;
        }
    }
}

