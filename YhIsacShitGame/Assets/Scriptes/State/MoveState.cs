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
        public override void Enter(BaseObject _baseObject)
        {
            base.Enter(_baseObject);
        }

        public override void Enter(System.Numerics.Vector3 _position)
        {
            base.Enter(_position);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
