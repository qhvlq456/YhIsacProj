using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

[System.Serializable]
public class TileData : BaseData
{
    public Define.Direction direction;

    // server 에서 할 일
    // 해당 타일이 적 길인지 아군 길인지, 데코인지 판단하는 값
    public Define.ElementType elementType;
    // 배치된 오브젝트 인덱스 후에 다른것들로 통합할 필요가 있다.
    public int batchIdx;

    public TileData() { }
    public TileData(int _index, string _name, Define.BaseType _type, Define.Direction _direction) : base(_index, _type, _name)
    {
        direction = _direction;
        batchIdx = 0;
        elementType = Define.ElementType.ENEMY;
    }

    public bool IsCharacter()
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
