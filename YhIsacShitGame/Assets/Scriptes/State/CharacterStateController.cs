using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class CharacterStateController : StateController
{
    protected List<State> stateList = new List<State>();
    public void AddState(State _state)
    {
        stateList.Add(_state);
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    // 후에 마우스 인풋 말고 다른 상태전환일때 사용
    public virtual void TransitionTo(State _nextState)
    {
        if (currentState != _nextState)
        {
            currentState.Exit();
            currentState = _nextState;
            currentState.Enter();
        }
    }
}
