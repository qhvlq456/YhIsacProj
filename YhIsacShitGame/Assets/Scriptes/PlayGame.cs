using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Play
{
    /// <summary>
    /// intro -> loading -> game flow 클래스
    /// </summary>
    public class PlayGame : ScheduledTask
    {
        public PlayGame(float duration) : base(duration)
        {

        }

        public override void Execute()
        {
            Debug.Log("게임 시작 작업 수행");
        }
    }
}

