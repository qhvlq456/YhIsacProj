using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Map
{
    [System.Serializable]
    public class TileData : BaseData, ILogger
    {
        public Define.Direction direction;

        // server 에서 할 일
        // 해당 타일이 적 길인지 아군 길인지, 데코인지 판단하는 값
        public Define.ElementType elementType;
        // 배치된 오브젝트 인덱스 후에 다른것들로 통합할 필요가 있다.
        public int batchIdx;

        public bool IsCharacter()
        {
            bool ret = false;
            return ret;
        }

        public override void Delete()
        {
            // 이니셜라이즈 또는 null처리
        }

        public override void Update(BaseData _baesData)
        {
            // 변경 상항들을 재 정의
        }
        public void Logger()
        {
            Debug.LogWarningFormat("Tile Data \n index : {0}, type : {1}", index, type);
        }
    }
}
