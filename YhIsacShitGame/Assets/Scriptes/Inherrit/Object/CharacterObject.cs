using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace YhProj.Game.Character
{
    public class CharacterObject : BaseObject
    {
        [SerializeField]
        protected NavMeshAgent agent;

        [SerializeField]
        protected Animator animator;

        // hero는 타일 위치, enemy : start postion ~ end postion으로 이동
        public override void Create<T>(T _data)
        {
            gameData = _data as CharacterData;
        }
        public override void Delete()
        {
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(BaseType.character, transform);
        }
    }
}
