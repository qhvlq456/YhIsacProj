namespace YhProj
{
    public abstract class State
    {
        public abstract void Update();
        public abstract void Exit();
        public abstract void Enter();
        //public abstract void OnInput();
        //public abstract void OnTrigger();
    }
}
