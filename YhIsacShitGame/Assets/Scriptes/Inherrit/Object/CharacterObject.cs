using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using YhProj.Game.Map;

namespace YhProj.Game.Character
{
    public class CharacterObject : MonoBehaviour, ISelectable, IGrid
    {
        public CharacterData characterData;
        [SerializeField]
        protected NavMeshAgent agent;

        [SerializeField]
        protected Animator animator;

        public GridData GridData { get; set; }

        // hero는 타일 위치, enemy : start postion ~ end postion으로 이동
        public virtual void Create<T>(T _characterData) where T : CharacterData
        {
            characterData = _characterData;
        }
        public virtual void Update()
        {

        }
        public virtual void Delete()
        {
            Managers.Instance.GetManager<ObjectPoolManager>().Retrieve(BaseType.character, transform);
        }

        public virtual void Select()
        {
            throw new NotImplementedException();
        }

        public virtual void Unselect()
        {
            throw new NotImplementedException();
        }
    }
}
