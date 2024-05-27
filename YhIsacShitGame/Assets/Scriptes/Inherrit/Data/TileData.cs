namespace YhProj.Game.Map
{
    [System.Serializable]
    public class TileData : GameData
    {
        public Define.Direction direction;

        // server 에서 할 일
        // 해당 타일이 적 길인지 아군 길인지, 데코인지 판단하는 값
        public ElementType elementType;
        // 배치된 오브젝트 인덱스 후에 다른것들로 통합할 필요가 있다.
        public int batchIdx;
    }
}
