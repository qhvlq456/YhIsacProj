namespace YhProj.Game
{
    // 모든 게임 데이터의 베이스의 타입 // base type -> subType 으로 간다, 필드 오브젝트만
    public enum BaseType
    {
        NONE,
        TILE,
        character,
        BUILD,
        accessory,
        item,
        COUNT
    }
    public enum ElementType
    {
        MINE,
        ENEMY,
        DECO
    }
    public class GameData
    {
        public int index;
        public BaseType type;
        public string name;
        public string resName;
    }
}

