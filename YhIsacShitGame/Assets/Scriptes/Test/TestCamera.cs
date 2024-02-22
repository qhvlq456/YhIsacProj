using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 후에 카메라 컨트롤러를 만들어 구현할 것임 이건 인터페이스로 할까도 생각중
public class TestCamera : MonoBehaviour
{
    // 후에 카메라 부분으로 빼야 할 듯?
    private readonly float referenceWidth = 1920f;
    private readonly float pixelsToUnit = 100f;

    public Transform target;
    // 변수명 추후에 수정 이게 왜 xoffset인지 모르겠음
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
