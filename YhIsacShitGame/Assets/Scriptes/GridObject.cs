using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Map
{
    public class GridObject : BaseObject
    {
        public override void Create<T>(T _data)
        {
            gameData = _data as GridData;
        }
        public override void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}

