namespace YhProj
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // ���ÿ� ���� �ൿ�� �����ؾ� ��
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
            // �ʱ�ȭ ����
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

