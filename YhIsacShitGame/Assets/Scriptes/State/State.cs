using UnityEngine;

namespace YhProj.Game.State
{
    public class State
    {
        public virtual void Update() { }
        public virtual void Exit() { }
        public virtual void Enter(BaseObject _baseObject) { }
        public virtual void Enter(Vector3 _position) { }
        //public abstract void OnInput();
        //public abstract void OnTrigger();
    }
}
