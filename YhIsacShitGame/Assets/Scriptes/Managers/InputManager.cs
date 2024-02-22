using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YhProj;

public class InputManager : BaseManager
{
    // 콜라이더를 넣어서 일단 드래그 할 수 있게 만들긴 해야함
    // 드래그 영역 막아야 함
    StateController stateController;
    Transform target;

    public InputManager(Transform _target)
    {
        // 보류 
        target = _target;
    }
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
                EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
                EventMediator.Instance.OnLoadSequenceEvent += LoadPlayerEvent;
                // 여기서 target attach를 set하여 순서를 정할 것임
                stateController = new EditStateController(target);
                break;
        }
    }
    public override void Update()
    {
        // ui가 없거나 활성화된 UI가 없을 때만 드래그 이벤트 처리
        // "마우스 포인터가 UI 요소 위에 있지 않고, 동시에 활성화된 UI가 없을 때" 
        // 원래는 그냥 isactiveui만 사용하려 했느나 간헐적으로 드래그가 되는 현상으로 인해 조건문 두개로 통합
        
        // 이 부분은 보류
        //if (!IsPointerOverUIObject() && !Managers.Instance.GetManager<UIManager>().IsActiveUI)

        if(!Managers.Instance.GetManager<UIManager>().IsActiveUI)
        {
            stateController.OnEnter();
            stateController.Update();
            stateController.OnExit();
        }
    }
    public override void Delete()
    {
        EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
    }

    public void LoadPlayerEvent(PlayerInfo _playerInfo)
    {
        // camera controller를 만들어 관리하는 방법으로 할 것임 statecontroller랑 비슷하다고 생각하면 될듯??
        Debug.LogError("InputManager LoadPlayerEvent!!");
    }

    // 이 부분은 보류
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
