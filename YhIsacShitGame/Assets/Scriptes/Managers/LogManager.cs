using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class LogManager : BaseManager
{
    public override void Load(Define.GameMode _gameMode)
    {
        Debug.LogError($"log manager load");
    }

    public override void Update()
    {
        Debug.LogError($"log manager update");
    }
    public override void Delete()
    {
        Debug.LogError($"log manager delete");
    }
}
