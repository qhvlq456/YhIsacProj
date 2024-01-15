using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 후에 카메라 컨트롤러를 만들어 구현할 것임 이건 인터페이스로 할까도 생각중
public class TestCamera : MonoBehaviour
{
    public Transform target;
    // 변수명 추후에 수정 이게 왜 xoffset인지 모르겠음
    public Vector3 xOffset = Vector3.up * 10;

    private void Awake()
    {
        // test set
        Camera cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 16;

        transform.eulerAngles = new Vector3(90, 0, 0);
    }
    private void Update()
    {
        transform.position = target.position + xOffset;
    }

    IEnumerator CoWaitTargetSet()
    {
        yield return null;


    }
}
