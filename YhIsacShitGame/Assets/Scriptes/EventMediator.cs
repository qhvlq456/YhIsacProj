using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * ���� �Ŵ��� Ŭ������ �ٸ� �Ŵ��� Ŭ�������� �̺�Ʈ�� �߰��ϴ� Ŭ����
 * ���� ������ �ִ� �̱����̿��� �Ѵ�
 */
public static class EventMediator
{
    public static event Action<int> OnPlayerLevelChange;
    public static event Action<PlayerInfo> OnLoadSequenceEvent;

    #region Player Levelup Event
    public static void RelayPlayerLevelChange(int _data)
    {
        OnPlayerLevelChange?.Invoke(_data);
    }
    #endregion

    #region Load sequence event
    public static void LoadSequnceEvent(PlayerInfo _playerInfo)
    {
        OnLoadSequenceEvent?.Invoke(_playerInfo);
    }
    #endregion

}
