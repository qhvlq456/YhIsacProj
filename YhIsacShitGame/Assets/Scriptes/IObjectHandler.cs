using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game
{
    public interface IObjectHandler
    {
        void Move();
        void Attack();
        void Hurt();
    }
}
