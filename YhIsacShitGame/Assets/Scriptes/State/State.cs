using System.Numerics;

namespace YhProj
{
    public class State
    {
        public virtual void Update() { }
        public virtual void Exit() { }
        public virtual void Enter() { }
        public virtual void Enter(Vector3 _position) { }
        //public abstract void OnInput();
        //public abstract void OnTrigger();
    }
}
