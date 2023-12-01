using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;


public class TileObject : BaseObject, YhProj.ILogger
{
    TileData tileData;
    //public Renderer renderer;
    public override void Load<T>(T _baseData)
    {
        tileData = _baseData as TileData;
    }
    public override void Delete()
    {
        gameObject.SetActive(false);
        tileData.Delete();
    }
    public void Renderer()
    {

    }
    public void Logger()
    {
        
    }
}
