using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Play;
using YhProj.Game.Map;

namespace YhProj.Game.Character
{
    public class HeroObject : CharacterObject, IAttack
    {
        // 생성 즉 배치가 되었을 때
        public override void Create<T>(T _data)
        {
            base.Create(_data);

        }
        public void Attack(CharacterObject _characterObject)
        {
            throw new System.NotImplementedException();
        }
    }
}

