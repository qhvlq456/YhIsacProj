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
                // 여기서 target attach를 set하여 순서를 정할 것임
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
