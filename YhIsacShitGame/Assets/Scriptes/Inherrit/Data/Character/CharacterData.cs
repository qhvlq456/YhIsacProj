namespace YhProj.Game.Character
{
    [System.Serializable]
    public class CharacterData : BaseData
    {
        public int health;
        public int armor;
        public int power;
        public int range;
        public Define.ElementType elementType;

        public override void Update()
        {

        }

        public override void Delete()
        {

        }
    }
}
