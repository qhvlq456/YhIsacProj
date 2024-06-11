using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj.Game.Character;

namespace YhProj.Game.YhEditor
{
    public class CharacterEditor : BaseEditor
    {
        private ICharacterFactory characterFacotry;
        private List<CharacterObject> characterObjectList = new List<CharacterObject>();
        public override void Initialize()
        {
            BoxCollider bottomColider = GameUtil.AttachObj<BoxCollider>("Bottom");
            bottomColider.size = new Vector3(100, 0, 100);
        }

        public override void Create(GameData _gameData)
        {
            CharacterObject charObject = null;
            CharacterData charData = _gameData as CharacterData;
            
            switch (charData.elementType)
            {
                case ElementType.mine:
                    characterFacotry = new HeroFactory();
                    break;
                case ElementType.enemy:
                    characterFacotry = new EnemyFactory();
                    break;
                default:
                    Debug.LogError($"characterData is Not ElementType : {charData.elementType}");
                    break;
            }

            charObject = characterFacotry.Create(charData);
            charObject.transform.SetParent(root);
            // 일단 기획에 없음으로 zero
            charObject.transform.localPosition = Vector3.zero;
            characterObjectList.Add(charObject);
        }
        public override void Delete(GameData _gameData)
        {
            if (_gameData is CharacterData charData)
            {
                CharacterObject charObject = characterObjectList.Where(x => x.characterData.index == _gameData.index).First();
                charObject?.Delete();
            }
            else
            {

            }
        }

        public override void Dispose()
        {
            
        }

        public override void Update()
        {
            
        }

    }
}
