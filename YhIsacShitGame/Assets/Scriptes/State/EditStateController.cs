using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YhProj.Game.State
{
    /// <summary>
    /// 멥툴에 대한 상태 클래스를 컨트롤 하는 클래스
    /// </summary>
    public class EditStateController : StateController
    {
        public Transform target;
        public float moveSpeed = 4.5f;
        private float clickThreshold = 0.1f; // 클릭 판별을 위한 시간 임계값

        Dictionary<Type, State> stateDic = new Dictionary<Type, State>();
        Vector3 mouseDownPosition = Vector3.zero;

        public EditStateController() { }

        public EditStateController(Transform _target)
        {
            currentState = GetState<NoneState>();
            target = _target;
        }

        /// <summary>
        /// state 를 리턴하는 함수
        /// </summary>
        /// <typeparam name="T"> 가져오고 싶은 상태 클래스 </typeparam>
        /// <param name="args"> 새롭게 생성 될 경우 해당 매개변수 </param>
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

        // 1. drag 상태가 아닐때 select를 할 수 있음
        // 2. drag 상태일 때 아무것도 못 함

        // 1. 드래그를 판별할 수 없음
        // 2. 드래그시 오브젝트를 클릭 처리해선 안됌
        // 3. exit의 활용성이 없음
        // 4. update의 활용성이 없음

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
            // 이부분 추후에 수정 필요 간헐적으로 발생함
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
                // select state가 존재하는 상태일 때 첫 클릭 시도시 move state임
                currentState = GetState<NoneState>();
            }

            currentState.Enter(null);
        }
        // move state도 설정하여야 함
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
                    // hold인지 select인지 시간초 보고 판단하여야 함
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
                // 전의 상태가 
                // 선택 상태 
                if (currentState is SelectState)
                {
                    BaseObject baseObject = hit.collider.GetComponent<BaseObject>();

                    if (baseObject != null)
                    {
                        currentState.Enter(baseObject);
                    }
                }
                // 홀드 상태
                else if (currentState is HoldState)
                {

                }
                else if (currentState is DragState)
                {

                }
                else
                {

                }
            }

            currentState = GetState<NoneState>();
        }
    }
}