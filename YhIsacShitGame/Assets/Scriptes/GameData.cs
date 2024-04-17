namespace YhProj.Game
{
    // 모든 게임 데이터의 베이스의 타입 // base type -> subType 으로 간다, 필드 오브젝트만
    public enum BaseType
    {
        none,
        tile,
        character,
        build,
        accessory,
        item,
        count
    }
    public enum ElementType
    {
        mine = 1,
        enemy,
        deco
    }
    public class GameData
    {
        public int index;
        public BaseType type;
        public string name;
        public string resName;
    }
}

