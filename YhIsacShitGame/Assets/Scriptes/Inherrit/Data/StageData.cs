using Newtonsoft.Json;
using YhProj;

[System.Serializable]
public class StageData
{
    public int lv { get; set; }
    public int stage { get; set; }
    public TileData[,] tileArr { get; set; }

    public float tileSize;
    public float xOffset;
    public float zOffset;
    public StageData() { }

    public StageData(int _stage, int _lv, TileData[,] _tileArr)
    {
        stage = _stage;
        lv = _lv;
        tileArr = _tileArr;
    }

    // 직렬화로인한 함수처리
    [JsonIgnore]
    public int Row => tileArr.GetLength(0);
    [JsonIgnore]
    public int Col => tileArr.GetLength(1);
}
