using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

/// <summary>
/// ������ ���� ���� Ŭ������ ��Ʈ�� �ϴ� Ŭ����
/// </summary>
public class EditStateController : StateController
{
    public Transform target;
    public float moveSpeed;
    public EditStateController() { }

    public EditStateController(Transform _target)
    {
        target = _target;
    }
    
    // 1. drag ���°� �ƴҶ� select�� �� �� ����
    // 2. drag ������ �� �ƹ��͵� �� ��

    // 1. �巡�׸� �Ǻ��� �� ����
    // 2. �巡�׽� ������Ʈ�� Ŭ�� ó���ؼ� �ȉ�
    // 3. exit�� Ȱ�뼺�� ����
    // 4. update�� Ȱ�뼺�� ����
    
    public override void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(currentState is SelectState)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out hit, Mathf.Infinity)) 
                {
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.white);

                    BaseObject baseObject = hit.collider.GetComponent<BaseObject>();
                    if (baseObject != null) 
                    {
                        currentState = new SelectState(baseObject);
                    }
                    else
                    {
                        currentState = new NoneState();
                    }
                }
            }
            else
            {
                currentState = new DragState(target, moveSpeed);
            }

            currentState.Enter();
        }

        if(Input.GetMouseButton(0))
        {
            // move state
            if (currentState is SelectState)
            {
                currentState = new MoveState();
            }
            // drag state
            else
            {
                currentState = new DragState(target, moveSpeed);
            }

            currentState.Enter();
            currentState.Update();
        }

        if(Input.GetMouseButtonUp(0))
        {
            currentState.Exit();
        }
    }
}
