using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace YhProj.Game.Character
{
    public class CharacterObject : BaseObject
    {
        [SerializeField]
        private NavMeshAgent agent;
        protected CharacterData characterData;

        public override void Load<T>(T _baseData)
        {
            characterData = _baseData as CharacterData;
        }

        public override void Delete()
        {
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(Define.BaseType.CHARACTER, transform);
        }
        public virtual int Attack()
        {
            int point = 0;

            return point;
        }
        public virtual void Hurt(int _damage)
        {

        }
    }
}
