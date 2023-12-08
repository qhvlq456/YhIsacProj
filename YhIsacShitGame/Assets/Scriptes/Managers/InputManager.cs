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
                // ���⼭ target attach�� set�Ͽ� ������ ���� ����
                if (testInput == null)
                {
                    testInput = Util.AttachObj<TestInput>("testInput");
                }
                break;
            case Define.GameMode.MAPTOOL:
                // ���⼭ target attach�� set�Ͽ� ������ ���� ����
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
