using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class InputManager : BaseManager
{
    // 콜라이더를 넣어서 일단 드래그 할 수 있게 만들긴 해야함
    // 드래그 영역 막아야 함

    [SerializeField]
    float moveSpeed = 3f;

    public Transform target { private set; get; }

    Ray ray = new Ray();

    Vector3 targetPosition = Vector3.zero;
    Vector3 downPosition = Vector3.zero;
    Vector3 dragPosition = Vector3.zero;

    StateController stateController;
    

    public override void Load(Define.GameMode _gameMode)
    {
        switch (_gameMode)
        {
            case Define.GameMode.EDITOR:
                break;
            case Define.GameMode.TEST:
                // 여기서 target attach를 set하여 순서를 정할 것임
                break;
            case Define.GameMode.MAPTOOL:
                TestCamera camera = Util.AttachObj<TestCamera>(Camera.main.gameObject, "Main Camera");

                target = Util.AttachObj<Transform>("target");
                camera.target = target;

                targetPosition = StaticDefine.START_POSITION;
                target.transform.position = targetPosition;

                // 여기서 target attach를 set하여 순서를 정할 것임
                stateController = new EditStateController();
                break;
        }
    }
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
            }

            downPosition = hit.point;
        }

        if (Input.GetMouseButton(0))
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
    public override void Delete()
    {
        
    }

}
