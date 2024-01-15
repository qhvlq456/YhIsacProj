using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

/// <summary>
/// 멥툴에 대한 상태 클래스를 컨트롤 하는 클래스
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
    
    // 1. drag 상태가 아닐때 select를 할 수 있음
    // 2. drag 상태일 때 아무것도 못 함

    // 1. 드래그를 판별할 수 없음
    // 2. 드래그시 오브젝트를 클릭 처리해선 안됌
    // 3. exit의 활용성이 없음
    // 4. update의 활용성이 없음
    
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
