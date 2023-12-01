using YhProj;
using System;

public class PlayerManager : BaseManager
{
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

    public PlayerInfo playerInfo;
    
    public override void Load()
    {
        
    }

    public override void Update()
    {
        
    }
    public override void Delete()
    {
        
    }
}
