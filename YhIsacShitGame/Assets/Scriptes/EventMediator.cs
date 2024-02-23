using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * 대장 매니저 클래스와 다른 매니저 클래스들의 이벤트를 중계하는 클래스
 * 씬에 박혀져 있는 싱글톤이여야 한다
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
