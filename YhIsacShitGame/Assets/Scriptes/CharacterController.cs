using System.Collections;
using System.Collections.Generic;

namespace YhProj.Game.Character
{
    public class CharacterController : IController
    {
        protected CharacterManager manager;

        protected List<CharacterObject> charObjectList = new List<CharacterObject>();

        public CharacterController(CharacterManager _manager)
        {
            manager = _manager;
        }
        public void Initialize()
        {

        }


        public virtual void Update()
        {

        }

        public void Dispose()
        {

        }
    }
    public sealed class EditorCharacterController : CharacterController
    {

        public EditorCharacterController(CharacterManager _manager) : base(_manager) { }


        
    }
}
