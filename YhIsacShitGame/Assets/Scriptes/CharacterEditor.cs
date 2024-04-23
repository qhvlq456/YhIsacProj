using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj.Game.Character;

namespace YhProj.Game.YhEditor
{
    public class CharacterEditor : BaseEditor
    {
        public CharacterEditor()
        {
            factory = new CharacterFactory();
        }
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
                    charObject = factory.Create<HeroObject>(charData);
                    break;
                case ElementType.enemy:
                    charObject = factory.Create<EnemyObject>(charData);
                    break;
                default:
                    Debug.LogError($"characterData is Not ElementType : {charData.elementType}");
                    break;
            }

            charObject.transform.SetParent(root);
            // 일단 기획에 없음으로 zero
            charObject.transform.localPosition = Vector3.zero;
            objectList.Add(charObject);
        }
        public override void Delete(GameData _gameData)
        {
            if (_gameData is CharacterData charData)
            {
                CharacterObject charObject = objectList.Select(x => x as CharacterObject).Where(x => x.characterData.index == _gameData.index).First();
                charObject?.Delete();
            }
            else
            {

            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Update()
        {
            
        }

    }
}
