using YhProj;
using static YhProj.Define;

namespace YhProj.Game
{
    public class BaseDataBuilder<T> : IBuilder<T> where T : BaseData, new()
    {
        protected T data;

        public BaseDataBuilder()
        {
            data = new T();
        }

        public BaseDataBuilder<T> SetIndex(int _idx)
        {
            data.index = _idx;
            return this;
        }
        public BaseDataBuilder<T> SetType(BaseType _type)
        {
            data.type = _type;
            return this;
        }
        public BaseDataBuilder<T> SetType(object _type)
        {
            if (int.TryParse(_type.ToString(), out int type))
            {
                data.type = (BaseType)type;
            }

            return this;
        }

        public BaseDataBuilder<T> SetName(string _name)
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
