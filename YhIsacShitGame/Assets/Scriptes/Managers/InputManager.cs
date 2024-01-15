using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        stateController.OnEnter();
        stateController.Update();
        stateController.OnExit();
    }
    public override void Delete()
    {
        EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
    }

    public void LoadPlayerEvent(PlayerInfo _playerInfo)
    {
        // camera controller를 만들어 관리하는 방법으로 할 것임 statecontroller랑 비슷하다고 생각하면 될듯??
        Managers.Instance.testCamera.target = target;
        Debug.LogError("InputManager LoadPlayerEvent!!");
    }
}
