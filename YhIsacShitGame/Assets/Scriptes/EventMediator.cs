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

    #region Player Levelup Event
    public void RelayPlayerLevelChange(int _data)
    {
        OnPlayerLevelChange?.Invoke(_data);
    }
    #endregion


}
