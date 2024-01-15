namespace YhProj
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // 선택에 대한 행동을 구현해야 함
    public class SelectState : State
    {
        BaseObject baseObject;

        public SelectState(BaseObject _baseObject) 
        { 
            if(_baseObject != null)
            {
                baseObject = _baseObject;
            }
        }
        public override void Enter()
        {
            // 초기화 설정
            Debug.Log("SelectState click enter!!");
        }
        public override void Update()
        {
            Debug.Log("SelectState click Update!!");
        }

        public override void Exit()
        {
            Debug.Log("SelectState click exit!!");
        }

    }
}

