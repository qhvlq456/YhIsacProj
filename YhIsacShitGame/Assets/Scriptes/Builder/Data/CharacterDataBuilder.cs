using YhProj.Game.Map;

namespace YhProj.Game.Character
{
    public class CharacterDataBuilder : GameDataBuilder<CharacterData>
    {
        public CharacterDataBuilder SetHealth(int _health)
        {
            data.health = _health;
            return this;
        }
        public CharacterDataBuilder SetArmor(int _armor)
        {
            data.armor = _armor;
            return this;
        }

        public CharacterDataBuilder SetElementType(ElementType _elementType)
        {
            data.elementType = _elementType;
            return this;
        }
        public CharacterDataBuilder SetRange(int _range)
        {
            data.range = _range;
            return this;
        }

        public override CharacterData Build()
        {
            return data;
        }
    }
}
