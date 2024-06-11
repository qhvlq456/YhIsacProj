using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.State
{
    public class MoveState : State
    {
        public MoveState() { }
        public static MoveState Create()
        {
            return new MoveState();
        }

        public override void Update()
        {
            base.Update();
        }
        public override void Enter(ISelectable _selectable)
        {
            base.Enter(_selectable);
        }

        public override void Enter(Vector3 _position)
        {
            base.Enter(_position);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
