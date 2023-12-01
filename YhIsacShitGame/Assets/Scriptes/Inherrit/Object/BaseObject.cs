using UnityEngine;

namespace YhProj
{
    public abstract class BaseObject : MonoBehaviour
    {
        public abstract void Load<T>(T _baseData) where T : BaseData;
        public abstract void Delete();

        public virtual void IdleAnimation() { }
        public virtual void ActiveAnimation() { }
        public virtual void EndAnimation() { }
    }
}
