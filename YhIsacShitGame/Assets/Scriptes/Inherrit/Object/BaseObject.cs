using UnityEngine;

namespace YhProj.Game
{
    public abstract class BaseObject : MonoBehaviour
    {
        public abstract void Create<T>(T _data);

        public abstract void Delete();

        public abstract void Update();
    }
}
