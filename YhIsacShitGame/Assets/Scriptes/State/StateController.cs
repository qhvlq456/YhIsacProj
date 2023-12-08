using System.Collections;
using System.Collections.Generic;
using System;

namespace YhProj
{
    public abstract class StateController
    {
        public State currentState { get; set; }
        protected List<State> stateList = new List<State>();
        public void AddState(State _state)
        {
            stateList.Add(_state);
        }
        public void OnEnter()
        {
            // ���°� ����� ������ ȣ��
        }

        public void OnExit()
        {
            // ���°� ����� ������ ȣ��
        }
        public abstract void Update();

        public virtual void TransitionTo(State _nextState)
        {
            if(currentState != _nextState)
            {
                currentState.Exit();
                currentState = _nextState;
                currentState.Enter();
            }
        }
    }
}

