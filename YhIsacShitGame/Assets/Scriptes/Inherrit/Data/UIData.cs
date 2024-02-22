using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIData : ScriptableObject
{
    public string resourceUIPath;
    public string mapToolUIPath;
    // 나중에 데이터가 잘 들어갔는지 보고싶음 주석 푸셈
    //[HideInInspector]
    public List<UIInfo> mainUIDataList = new List<UIInfo>();
    //[HideInInspector]
    public List<UIInfo> popupUIDataList = new List<UIInfo>();
    //[HideInInspector]
    public List<UIInfo> tooltipUIDataList = new List<UIInfo>();
    //[HideInInspector]
    public List<UIInfo> contextualUIDataList = new List<UIInfo>();
    //[HideInInspector]
    public List<UIInfo> testUIDataList = new List<UIInfo>();
}


