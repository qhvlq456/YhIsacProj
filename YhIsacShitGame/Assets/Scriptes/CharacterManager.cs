using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Character
{
    public class CharacterManager : BaseManager
    {
        private List<CharacterObject> characterObjectList = new List<CharacterObject>();
        private CharacterController characterController;
        private IFactory factory;

        public void SwitchFactory<T>() where T : IFactory, new()
        {
            factory = new T();
        }
        public override void Load(Define.GameMode _gameMode)
        {

        }
        public override void Update()
        {

        }
        public override void Delete()
        {
            foreach (CharacterObject characterObject in characterObjectList)
            {
                characterObject.Delete();
            }
        }
    }
}
