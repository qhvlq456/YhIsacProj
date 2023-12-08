using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class TestInput : MonoBehaviour
{
    // �ݶ��̴��� �־ �ϴ� �巡�� �� �� �ְ� ����� �ؾ���
    // �巡�� ���� ���ƾ� ��

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

    // ������ ���� ��ǲ�� �� ����
    // �������� obj�� �Ÿ� �� ���
    // �巡�׸� �ϰ� �Ǹ� �����̴µ� �� ������ �� �𸣰ڳ�
    // �巡�׸� �ϸ� �巡�� ��ġ�� ���̽� ��ġ���� �޾ƿ�
    // ���� ������ �巹�׽� �޾ƿ� ��ġ���� ���̸� ������
    // obj - �޾ƿ� ��ġ���� ���� = obj 
    // ��, obj��ġ���� = target pos(ī�޶� �ٶ� ��ġ��) - (drag pos (ray�� �� �޾ƿ� ��ġ��) - start ray pos (ó�� ray��))
    // �ٵ� �������� start ray pos = drag pos (ray�� �� �޾ƿ� ��ġ��) - (drag pos (ray�� �� �޾ƿ� ��ġ��) - start ray pos (ó�� ray��)) // �̰� �� �ϴ°���?
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

                // �ϴ� �̰� �� �ִ��� �� �𸣰ھ �ּ�
                //downPosition = dragPosition - distance;
            }
        }

        target.position = Vector3.Lerp(target.transform.position, targetPosition, moveSpeed);
        // �ؿ��κ��� ���Ŀ� ����
        target.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);

        if (Input.GetMouseButtonUp(0))
        {

        }
    }
}
