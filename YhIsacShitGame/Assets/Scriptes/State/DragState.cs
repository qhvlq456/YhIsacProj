using UnityEngine;

namespace YhProj.Game.State
{
    // object를 잡고 있는가? 
    public class DragState : State
    {
        float moveSpeed;

        // 아래 targetposion 빼고 다른 값들이 zero로 set되어 있어서 그럼
        Vector3 targetPosition = Vector3.zero;
        Vector3 downPosition = Vector3.zero;
        Vector3 dragPosition = Vector3.zero;

        Transform target;
        public DragState(Transform _taget, float _moveSpeed)
        {
            target = _taget;
            moveSpeed = _moveSpeed;
        }
        // 팩토리 메서드
        public static DragState Create(Transform _taget, float _moveSpeed)
        {
            return new DragState(_taget, _moveSpeed);
        }

        public override void Enter(BaseObject _baseObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
            }

            downPosition =  hit.point;
        }
        public override void Update()
        {
            // Handle dragging logic here
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);

                dragPosition = hit.point;
                Vector3 distance = dragPosition - downPosition;
                targetPosition = target.position - distance;

                // 일단 이건 왜 있는지 잘 모르겠어서 주석
                //downPosition = dragPosition - distance;
            }

            target.position = Vector3.Lerp(target.transform.position, targetPosition, moveSpeed);
            // 밑에부분은 추후에 수정
            // Update target position based on drag movement

            targetPosition = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        }
        public override void Exit()
        {
            Debug.Log("DragState: Drag Released!");
        }
    }
}
