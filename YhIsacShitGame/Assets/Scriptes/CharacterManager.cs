using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace YhProj.Game.Character
{
    public class CharacterManager : BaseManager, IDataHandler
    {
        public Transform root;

        private IController controller;
        private IFactory factory;

        private void SwitchFactory<T>() where T : IFactory, new()
        {
            factory = new T();
        }
        public override void Load(Define.GameMode _gameMode)
        {
            if (root == null)
            {
                root = GameUtil.AttachObj<Transform>("Charater");
                root.transform.position = Vector3.zero;
            }

            switch(_gameMode) 
            {
                case Define.GameMode.EDITOR:
                    controller = new EditorCharacterController(this);
                    //dataHandler = characterController;
                    break;
            }
        }
        public override void Update()
        {

        }
        public override void Delete()
        {
            //characterController.Save();
        }

        public void DataLoad()
        {
            throw new System.NotImplementedException();
        }

        public void DataSave<T>(params T[] _params) where T : BaseData
        {
            throw new System.NotImplementedException();
        }
    }
}
