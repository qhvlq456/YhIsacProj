using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public Transform target { protected get; set; }
    // 변수명 추후에 수정 이게 왜 xoffset인지 모르겠음
    public Vector3 xOffset = Vector3.zero;

    private void Awake()
    {
        xOffset = transform.position - target.position;

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
