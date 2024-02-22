using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        // ui�� ���ų� Ȱ��ȭ�� UI�� ���� ���� �巡�� �̺�Ʈ ó��
        // "���콺 �����Ͱ� UI ��� ���� ���� �ʰ�, ���ÿ� Ȱ��ȭ�� UI�� ���� ��" 
        // ������ �׳� isactiveui�� ����Ϸ� �ߴ��� ���������� �巡�װ� �Ǵ� �������� ���� ���ǹ� �ΰ��� ����
        
        // �� �κ��� ����
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
        // camera controller�� ����� �����ϴ� ������� �� ���� statecontroller�� ����ϴٰ� �����ϸ� �ɵ�??
        Debug.LogError("InputManager LoadPlayerEvent!!");
    }

    // �� �κ��� ����
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
