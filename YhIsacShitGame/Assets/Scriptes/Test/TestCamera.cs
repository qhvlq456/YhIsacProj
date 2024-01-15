using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ŀ� ī�޶� ��Ʈ�ѷ��� ����� ������ ���� �̰� �������̽��� �ұ ������
public class TestCamera : MonoBehaviour
{
    public Transform target;
    // ������ ���Ŀ� ���� �̰� �� xoffset���� �𸣰���
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
