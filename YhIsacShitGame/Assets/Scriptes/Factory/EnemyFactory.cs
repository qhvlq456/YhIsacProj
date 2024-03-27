using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game;

public class EnemyFactory : IFactory
{
    public V Create<T, V>(T _data)
        where T : BaseData
        where V : BaseObject
    {
        throw new System.NotImplementedException();
    }
}
