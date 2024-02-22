using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// ������ ���� ���� Ŭ������ ��Ʈ�� �ϴ� Ŭ����
/// </summary>
public class EditStateController : StateController
{
    public Transform target;
    public float moveSpeed = 4.5f;
    private float clickThreshold = 0.1f; // Ŭ�� �Ǻ��� ���� �ð� �Ӱ谪

    Dictionary<Type, State> stateDic = new Dictionary<Type, State>();
    Vector3 mouseDownPosition = Vector3.zero;

    public EditStateController() { }

    public EditStateController(Transform _target)
    {
        currentState = GetState<NoneState>();
        target = _target;
    }
    
    /// <summary>
    /// state �� �����ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T"> �������� ���� ���� Ŭ���� </typeparam>
    /// <param name="args"> ���Ӱ� ���� �� ��� �ش� �Ű����� </param>
    /// <returns></returns>
    T GetState<T>(params object[] args) where T : State
    {
        T ret = null;

        if (stateDic.ContainsKey(typeof(T)))
        {
            ret = stateDic[typeof(T)] as T;
        }
        else
        {
            ret = (T)typeof(T).GetMethod("Create").Invoke(null, args);
            stateDic.Add(typeof(T), ret);
        }

        return ret;
    }

    // 1. drag ���°� �ƴҶ� select�� �� �� ����
    // 2. drag ������ �� �ƹ��͵� �� ��

    // 1. �巡�׸� �Ǻ��� �� ����
    // 2. �巡�׽� ������Ʈ�� Ŭ�� ó���ؼ� �ȉ�
    // 3. exit�� Ȱ�뼺�� ����
    // 4. update�� Ȱ�뼺�� ����
    
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition = Input.mousePosition;
            ChangeStateOnMouseDown();
        }

        if (Input.GetMouseButton(0))
        {
            if (IsMouseDragging())
            {
                ChangeStateOnMouseDrag();
            }
            else
            {
                ChangeStateOnMouseHold();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ChangeStateOnMouseUp();
        }
    }
    private bool IsMouseDragging()
    {
        // �̺κ� ���Ŀ� ���� �ʿ� ���������� �߻���
        Vector3 currentMousePosition = Input.mousePosition;
        float distanceX = Mathf.Abs(currentMousePosition.x - mouseDownPosition.x);
        float distanceY = Mathf.Abs(currentMousePosition.y - mouseDownPosition.y);

        return distanceX >= clickThreshold || distanceY >= clickThreshold;
    }

    private void ChangeStateOnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // select state, drag state 
            currentState = GetState<DragState>(target, moveSpeed);
        }
        else
        {
            // select state�� �����ϴ� ������ �� ù Ŭ�� �õ��� move state��
            currentState = GetState<NoneState>();
        }

        currentState.Enter(null);
    }
    // move state�� �����Ͽ��� ��
    private void ChangeStateOnMouseDrag()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            currentState = GetState<DragState>(target, moveSpeed);
        }
        else
        {
            currentState = GetState<NoneState>();
        }

        currentState.Update();
    }

    private void ChangeStateOnMouseHold()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            BaseObject baseObject = hit.collider.GetComponent<BaseObject>();

            if (baseObject != null)
            {
                // hold���� select���� �ð��� ���� �Ǵ��Ͽ��� ��
                currentState = GetState<SelectState>();
            }
        }
        else
        {
            currentState = GetState<NoneState>();
        }

        currentState.Update();
    }
    private void ChangeStateOnMouseUp()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // ���� ���°� 
            // ���� ���� 
            if(currentState is SelectState)
            {
                BaseObject baseObject = hit.collider.GetComponent<BaseObject>();

                if (baseObject != null)
                {
                    currentState.Enter(baseObject);
                }
            }
            // Ȧ�� ����
            else if(currentState is HoldState)
            {

            }
            else if(currentState is DragState)
            {

            }
            else
            {

            }
        }

        currentState = GetState<NoneState>();
    }
}
