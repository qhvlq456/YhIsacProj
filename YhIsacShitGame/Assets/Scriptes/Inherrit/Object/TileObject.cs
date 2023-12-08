using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;


public class TileObject : BaseObject, YhProj.ILogger
{
    TileData tileData;
    //public Renderer renderer;

    // 흠 이것도 object로 만들어서 다음 tile obj, etcobj 등등 나눈것도 괜찮을 것 같음 
    // 
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
