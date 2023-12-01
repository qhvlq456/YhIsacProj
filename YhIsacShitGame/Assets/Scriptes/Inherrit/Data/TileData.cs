using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

[System.Serializable]
public class TileData : BaseData
{
    public Define.Direction direction;
    // true = 배치 가능한 길, false = 배치 불가능한 길
    public bool isSection;
    // server 에서 할 일
    // 해당 타일의 건물 인덱스 만약 존재한다면 0보다 큰 값 , 없다면 == 0
    public int buildIdx;
    // 해당 타일의 캐릭터 인덱스 만약 존재한다면 0보다 큰 값, 없다면 == 0
    public int characterIdx;
    // 배치가 가능한 자리인가?
    public int batch;
    public bool IsCharacter()
    {
        bool ret = false;
        return ret;
    }
    public bool IsBuild()
    {
        bool ret = false;
        return ret;
    }
    public override void Load()
    {
        
    }

    public override void Delete()
    {
        
    }

    public override void Update()
    {
        
    }
    public override void Logger()
    {
        Debug.LogWarningFormat("Tile Data \n index : {0}, type : {1}", index, type);
    }
}
