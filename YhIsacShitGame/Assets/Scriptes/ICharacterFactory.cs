using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Character
{
    public interface IFactorySelector
    {
        ICharacterFactory SelectFactory(ElementType elementType);
    }
    public interface ICharacterFactory
    {
        public CharacterObject Create(CharacterData _charData);
    }
    public class FactorySelector : IFactorySelector
    {
        private readonly Dictionary<ElementType, ICharacterFactory> factoryMap;

        public FactorySelector()
        {
            factoryMap = new Dictionary<ElementType, ICharacterFactory>
            {
                { ElementType.mine, new HeroFactory() },
                { ElementType.enemy, new EnemyFactory() }
            };
        }

        public ICharacterFactory SelectFactory(ElementType elementType)
        {
            factoryMap.TryGetValue(elementType, out ICharacterFactory factory);
            return factory;
        }
    }
    public class HeroFactory : ICharacterFactory
    {
        public CharacterObject Create(CharacterData _charData)
        {
            // ObjectPoolManager를 사용하여 HeroObject를 생성하고 반환
            Transform trf = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(_charData.type, _charData.resName);
            return trf.GetComponent<HeroObject>();
        }
    }
    public class EnemyFactory : ICharacterFactory
    {
        public CharacterObject Create(CharacterData _charData)
        {
            // EnemyObject 생성 로직 추가
            Transform trf = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(_charData.type, _charData.resName);
            return trf.GetComponent<EnemyObject>();
        }
    }

}
