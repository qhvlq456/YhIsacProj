using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YhProj;

public class MapToolUI : TestUI
{
    public int drawRow { private set; get; }
    public int drawCol { private set; get; }
    public float xOffset;
    public float yOffset;



    // camera 후 input이 먼저인것 같은데;;
    [SerializeField]
    List<Button> buttonList = new List<Button>();



}
