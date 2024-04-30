using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Play
{
    public class Loading : ScheduledTask
    {
        public Loading(float duration) : base(duration)
        {
        }

        public override void Execute()
        {
            Debug.Log("Loading 작업 수행");
        }
    }
}
