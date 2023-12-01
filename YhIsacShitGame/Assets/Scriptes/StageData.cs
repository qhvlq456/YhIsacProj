[System.Serializable]
public class StageData
{
    public int lv { get; set; }
    public int stage { get; set; }
    public int r, c;
    public TileData[,] tileArr { get; set; }

    public float xOffset;
    public float zOffset;
    public StageData() { }

    public StageData(int _lv, TileData[,] _tileArr)
    {
        lv = _lv;
        tileArr = _tileArr;
    }
}
