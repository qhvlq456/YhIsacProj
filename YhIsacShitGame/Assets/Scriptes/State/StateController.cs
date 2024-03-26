namespace YhProj.Game.State
{
    public abstract class StateController
    {
        public State currentState { get; set; }
        public virtual void OnEnter()
        {
            // 상태가 변경될 때마다 호출
        }

        public virtual void OnExit()
        {
            // 상태가 변경될 때마다 호출
        }
        public virtual void Update()
        {
            // 상태가 업데이트 될 때마다 호출
        }

    }
}

