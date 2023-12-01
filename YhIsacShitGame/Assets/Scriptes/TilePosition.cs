using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

public class TilePosition
{
    public int stage;
    public List<float> xPosList = new List<float>();
    public List<float> zPosList = new List<float>();

    public TilePosition(int _stage, List<float> _xPosList, List<float> _zPosList)
    {
        stage = _stage;
        xPosList = _xPosList;
        zPosList = _zPosList;
    }
    public Vector3 GetIndexByPosition(int _r, int _c)
    {
        return new Vector3(xPosList[_r], StaticDefine.TILE_YPOSITION, zPosList[_c]);
    }
}
