using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class InputManager : BaseManager
{
    TestInput testInput;

    public override void Load(Define.GameMode _gameMode)
    {
        switch (_gameMode)
        {
            case Define.GameMode.EDITOR:
                break;
            case Define.GameMode.TEST:
                // 여기서 target attach를 set하여 순서를 정할 것임
                if (testInput == null)
                {
                    testInput = Util.AttachObj<TestInput>("testInput");
                }
                break;
            case Define.GameMode.MAPTOOL:
                // 여기서 target attach를 set하여 순서를 정할 것임
                if (testInput == null)
                {
                    testInput = Util.AttachObj<TestInput>("testInput");

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
