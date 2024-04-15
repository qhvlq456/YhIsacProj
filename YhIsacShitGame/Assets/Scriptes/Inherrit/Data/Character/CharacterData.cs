using System.Collections.Generic;

namespace YhProj.Game.Character
{
    public enum AttributeType
    {
        water, // 물
        land, // 땅
        fire, // 불
        lightning, // 번개
        grass // 풀
    }
    [System.Serializable]
    public class CharacterData : GameData
    {
        public int health;
        public int armor;
        public int power;
        public int range;
        public float moveSpeed;
        public ElementType elementType;
        public AttributeType attribute;
    }

    [System.Serializable]
    public class HeroData : CharacterData
    {

    }

    [System.Serializable]
    public class EnemyData : CharacterData
    {
        
    }
}
