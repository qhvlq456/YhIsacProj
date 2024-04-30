using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Play
{
    public class Intro : ScheduledTask
    {
        public Intro(float duration) : base(duration)
        {
        }

        public override void Execute()
        {
            Debug.Log("Intro 작업 수행");
        }
    }
}

