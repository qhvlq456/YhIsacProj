using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Play
{
    public interface IGameFlow
    {
        public void OnStart();
        public void OnUpdate();
        public void OnEnd();
    }
}
