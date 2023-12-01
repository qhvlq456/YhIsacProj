using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class InputManager : BaseManager
{
    TestInput testInput;

    public override void Load()
    {
        switch (Managers.Instance.gameMode)
        {
            case Define.GameMode.ANDROID:
                break;
            case Define.GameMode.IOS:
                break;
            case Define.GameMode.EDITOR:
                break;
            case Define.GameMode.TEST:
                // ���⼭ target attach�� set�Ͽ� ������ ���� ����
                if(testInput == null)
                {
                    testInput = Util.AttachObj<TestInput>(testInput.gameObject);
                    
                }
                break;
        }
    }
    public override void Update()
    {
        
    }
    public override void Delete()
    {
        
    }

}
