using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Map;

namespace YhProj.Game.Character
{
    public class CharacterController : BaseController<MapManager>
    {
        public virtual void LoadCharacter()
        {

        }
        public virtual void UpdateCharacter()
        {

        }
        public virtual void DeleteCharacter()
        {

        }
    }

    public class CharacterController<T> : BaseController<CharacterManager> where T : CharacterObject
    {
        protected List<T> objectList = new List<T>();

        public CharacterController(BaseManager _characterManager) : base(_characterManager) 
        {
            manager = _characterManager as CharacterManager;
        }
    }

    public class EditorCharacterController : CharacterController<CharacterObject>, IJson
    {
        public EditorCharacterController(BaseManager _baseManager) : base(_baseManager) { }

        public void LoadJson()
        {
            throw new System.NotImplementedException();
        }

        public void SaveJson<T>(T _data)
        {
            throw new System.NotImplementedException();
        }
    }
}
