using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

[System.Serializable]
public class TileData : BaseData
{
    public Define.Direction direction;
    // true = ��ġ ������ ��, false = ��ġ �Ұ����� ��
    public bool isSection;
    // server ���� �� ��
    // �ش� Ÿ���� �ǹ� �ε��� ���� �����Ѵٸ� 0���� ū �� , ���ٸ� == 0
    public int buildIdx;
    // �ش� Ÿ���� ĳ���� �ε��� ���� �����Ѵٸ� 0���� ū ��, ���ٸ� == 0
    public int characterIdx;
    // ��ġ�� ������ �ڸ��ΰ�?
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
