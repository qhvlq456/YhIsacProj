using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class TestInput : MonoBehaviour
{
    // 콜라이더를 넣어서 일단 드래그 할 수 있게 만들긴 해야함
    // 드래그 영역 막아야 함

    [SerializeField]
    float moveSpeed = 3f;

    Transform target;

    Ray ray = new Ray();

    Vector3 targetPosition = Vector3.zero;
    Vector3 downPosition = Vector3.zero;
    Vector3 dragPosition = Vector3.zero;

    void Awake()
    {
        target = Util.AttachObj<Transform>();

        targetPosition = StaticDefine.START_POSITION;
        target.transform.position = targetPosition;
    }

    // 원점은 내가 인풋을 한 곳임
    // 원점에서 obj의 거리 값 계산
    // 드래그를 하게 되면 움직이는데 이 원리를 잘 모르겠네
    // 드래그를 하면 드래그 위치에 레이쏴 위치값을 받아옴
    // 시작 지점과 드레그시 받아온 위치값의 차이를 가져옴
    // obj - 받아온 위치값의 차이 = obj 
    // 즉, obj위치값은 = target pos(카메라가 바라볼 위치값) - (drag pos (ray를 쏴 받아온 위치값) - start ray pos (처음 ray값))
    // 근데 마지막에 start ray pos = drag pos (ray를 쏴 받아온 위치값) - (drag pos (ray를 쏴 받아온 위치값) - start ray pos (처음 ray값)) // 이걸 왜 하는거지?
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
            }

            downPosition = hit.point;
        }

        if(Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
        }

        target.position = Vector3.Lerp(target.transform.position, targetPosition, moveSpeed);
        // 밑에부분은 추후에 수정
        target.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);

        if (Input.GetMouseButtonUp(0))
        {

        }
    }
}
