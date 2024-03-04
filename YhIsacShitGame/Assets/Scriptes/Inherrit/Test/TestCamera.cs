using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ŀ� ī�޶� ��Ʈ�ѷ��� ����� ������ ���� �̰� �������̽��� �ұ ������
public class TestCamera : MonoBehaviour
{
    // �Ŀ� ī�޶� �κ����� ���� �� ��?
    private readonly float referenceWidth = 1920f;
    private readonly float pixelsToUnit = 100f;

    public Transform target;
    // ������ ���Ŀ� ���� �̰� �� xoffset���� �𸣰���
    public Vector3 xOffset = Vector3.up * 10;

    private void Awake()
    {
        // test set
        Camera cam = GetComponent<Camera>();
        cam.orthographic = true;

        transform.eulerAngles = new Vector3(90, 0, 0);

        AdjustCameraAspect(cam);
    }
    private void Update()
    {
        transform.position = target.position + xOffset;
    }
    void AdjustCameraAspect(Camera _camera)
    {
        float targetAspect = referenceWidth / Screen.height;
        float desiredSize = referenceWidth / (2 * pixelsToUnit);

        _camera.aspect = targetAspect;
        _camera.orthographicSize = desiredSize;
    }
    IEnumerator CoWaitTargetSet()
    {
        yield return null;


    }
}
