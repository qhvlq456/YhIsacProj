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



    // camera �� input�� �����ΰ� ������;;
    [SerializeField]
    List<Button> buttonList = new List<Button>();



}
