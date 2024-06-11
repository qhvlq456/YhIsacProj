using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.State
{
    public class NoneState : State
    {
        public NoneState() { }
        public static NoneState Create()
        {
            return new NoneState();
        }

        public override void Enter(ISelectable _selectable)
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
