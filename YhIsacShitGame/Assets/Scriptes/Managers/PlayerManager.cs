using YhProj;
using System;

[Serializable]
public class PlayerInfo
{
    int lv;
    public int Lv
    {
        get
        {
            return lv;
        }
        set
        {
            lv = value;
            EventMediator.Instance.RelayPlayerLevelChange(lv);
        }
    }
    public int gold;
    public int cash;
}

public class PlayerManager : BaseManager
{
    public PlayerInfo playerInfo { get; private set; }

    public PlayerManager(PlayerInfo _playerInfo)
    {
        playerInfo = _playerInfo;
    }

    public override void Load(Define.GameMode _gameMode)
    {
        EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
        EventMediator.Instance.OnLoadSequenceEvent += LoadPlayerEvent;
    }

    public override void Update()
    {
        
    }
    public override void Delete()
    {
        EventMediator.Instance.OnLoadSequenceEvent -= LoadPlayerEvent;
    }
    public void LoadPlayerEvent(PlayerInfo _playerInfo)
    {
        UnityEngine.Debug.LogError("LoadPlayerEvent!!");
    }
}
