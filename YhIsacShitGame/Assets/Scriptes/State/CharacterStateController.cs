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

    // �Ŀ� ���콺 ��ǲ ���� �ٸ� ������ȯ�϶� ���
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
