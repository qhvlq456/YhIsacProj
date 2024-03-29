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
        protected CharacterData characterData;

        public override void Create<T>(T _data)
        {
            characterData = _data as CharacterData;
        }

        public override void Update()
        {

        }
        public override void Delete()
        {
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(BaseType.character, transform);
        }
    }
}
