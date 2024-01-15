using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class InputManager : BaseManager
{
    // �ݶ��̴��� �־ �ϴ� �巡�� �� �� �ְ� ����� �ؾ���
    // �巡�� ���� ���ƾ� ��
    StateController stateController;
    Transform target;

    public InputManager(Transform _target)
    {
        // ���� 
        target = _target;
    }
    public override void Load(Define.GameMode _gameMode)
    {
        switch (_gameMode)
        {
            case Define.GameMode.EDITOR:
                break;
            case Define.GameMode.TEST:
                // ���⼭ target attach�� set�Ͽ� ������ ���� ����
                break;
            case Define.GameMode.MAPTOOL:
                EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
                EventMediator.Instance.OnLoadSequenceEvent += LoadPlayerEvent;
                // ���⼭ target attach�� set�Ͽ� ������ ���� ����
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
        // camera controller�� ����� �����ϴ� ������� �� ���� statecontroller�� ����ϴٰ� �����ϸ� �ɵ�??
        Managers.Instance.testCamera.target = target;
        Debug.LogError("InputManager LoadPlayerEvent!!");
    }
}
