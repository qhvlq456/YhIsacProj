using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * ���� �Ŵ��� Ŭ������ �ٸ� �Ŵ��� Ŭ�������� �̺�Ʈ�� �߰��ϴ� Ŭ����
 * ���� ������ �ִ� �̱����̿��� �Ѵ�
 */
public class EventMediator : Singleton<EventMediator>
{
    public event Action<int> OnPlayerLevelChange;
    public event Action<PlayerInfo> OnLoadSequenceEvent;

    #region Player Levelup Event
    public void RelayPlayerLevelChange(int _data)
    {
        OnPlayerLevelChange?.Invoke(_data);
    }
    #endregion

    #region Load sequence event
    public void LoadSequnceEvent(PlayerInfo _playerInfo)
    {
        OnLoadSequenceEvent?.Invoke(_playerInfo);
    }
    #endregion

}
