namespace YhProj.Game
{
    public interface IFactory
    {
        T Create<T>(GameData _data) where T : BaseObject;
        T Create<T>(GameData _data, UnityEngine.Vector3 _position) where T : BaseObject;
        T Create<T>(GameData _data, UnityEngine.Transform _parent) where T : BaseObject;
        T Create<T>(GameData _data, UnityEngine.Transform _parent, UnityEngine.Vector3 _position) where T : BaseObject;
    }

}

