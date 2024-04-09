namespace YhProj.Game
{
    public interface IFactory
    {
        T Create<T>(BaseData _data) where T : BaseObject;
        T Create<T>(BaseData _data, UnityEngine.Vector3 _position) where T : BaseObject;
        T Create<T>(BaseData _data, UnityEngine.Transform _parent) where T : BaseObject;
        T Create<T>(BaseData _data, UnityEngine.Transform _parent, UnityEngine.Vector3 _position) where T : BaseObject;
    }

}

