namespace YhProj
{
    public abstract class StateController
    {
        public State currentState { get; set; }
        public virtual void OnEnter()
        {
            // ���°� ����� ������ ȣ��
        }

        public virtual void OnExit()
        {
            // ���°� ����� ������ ȣ��
        }
        public abstract void Update();

    }
}

