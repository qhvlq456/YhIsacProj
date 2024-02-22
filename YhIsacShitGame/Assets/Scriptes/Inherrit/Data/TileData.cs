using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

[System.Serializable]
public class TileData : BaseData
{
    public Define.Direction direction;

    // server ���� �� ��
    // �ش� Ÿ���� �� ������ �Ʊ� ������, �������� �Ǵ��ϴ� ��
    public Define.RoadType roadType;
    // ��ġ�� ������Ʈ �ε��� �Ŀ� �ٸ��͵�� ������ �ʿ䰡 �ִ�.
    public int batchIdx;

    public TileData() { }
    public TileData(int _index, string _name, Define.BaseType _type, Define.Direction _direction) : base(_index, _type, _name)
    {
        direction = _direction;
        batchIdx = 0;
        roadType = Define.RoadType.ENEMY;
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
