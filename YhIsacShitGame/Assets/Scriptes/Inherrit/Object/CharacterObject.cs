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
        // destination 생각하여야 함
        public virtual void Move()
        {

        }
        // enemy 생각하여야 함
        public virtual int Attack(CharacterObject _enemyObj)
        {
            // 무기 등 악세서리에 대한 로직 합산 생각하여야 함
            int point = 0;

            return point;
        }
        public virtual void Hurt(int _damage)
        {
            // animaion 시간 보장하여야 함
            // 방어력에 대한 로직 생각하여야 함
            characterData.health -= _damage;
            
            if (characterData.health <= 0)
            {
                Delete();
            }
        }
    }
}
