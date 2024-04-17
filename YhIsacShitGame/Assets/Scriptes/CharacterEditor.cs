using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Character;
using YhProj.Game.Map;

namespace YhProj.Game.YhEditor
{
    public class CharacterEditor : BaseEditor
    {
        private CharDataHandler charDataHandler;

        public override IDataHandler<T> GetHandler<T>()
        {
            if (typeof(T) == typeof(CharDataHandler))
            {
                return charDataHandler as IDataHandler<T>;
            }
            else
            {
                // 다른 타입에 대한 핸들러를 반환하거나 예외 처리
                throw new System.NotSupportedException($"Handler for type {typeof(T)} is not supported.");
            }
        }
        public CharacterEditor()
        {
            factory = new CharacterFactory();
        }

        public override void Create(GameData _gameData)
        {

        }
        public override void Delete(GameData _gameData)
        {
            
        }

        public override void Dispose()
        {
            
        }

        public override void Initialize()
        {
            
        }

        public override void Update()
        {
            
        }

    }
}
