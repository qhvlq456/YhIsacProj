using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using YhProj;
using TMPro;
using System.Linq;


public class MapToolUI : BaseUI
{
    List<string> excpetionAttribute = new List<string>()
    {
        "index",
        "type",
    };

    [SerializeField]
    private readonly string mapToolCategoryPath = "UI/MapTool/MapToolCategory";

    [Header("Data")]
    // ���� Ŭ���ؼ� UI�� ǥ�õǰ��� ������Ʈ
    public EditorTileObject editorTileObject;

    [SerializeField]
    private Button clearBtn;

    [SerializeField]
    private TextMeshProUGUI infoText;

    // scorllview
    private Dictionary<string, MapToolCategoryUI> categoryUIDic = new Dictionary<string, MapToolCategoryUI>();

    [Header("ScrollView")]
    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private Transform contentTrf;

    [SerializeField]
    private Button applyBtn;

    [SerializeField]
    private Button exitBtn;

    private void Awake()
    {
        titleText.text = "MapToolUI";
        applyBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
        clearBtn.onClick.RemoveAllListeners();

        applyBtn.onClick.AddListener(ApplyBtnClick);
        exitBtn.onClick.AddListener(ExitBtnClick);
        clearBtn.onClick.AddListener(ClearBtnClick);

        foreach (var field in typeof(TileData).GetFields())
        {
            if(excpetionAttribute.Contains(field.Name))
            {
                continue;
            }

            MapToolCategoryUI mapToolCategoryUI = Util.InstantiateResource<MapToolCategoryUI>(mapToolCategoryPath);
            mapToolCategoryUI.transform.SetParent(contentTrf, false);
            mapToolCategoryUI.Set(field);

            categoryUIDic.Add(field.Name.ToLower(), mapToolCategoryUI);
        }
    }

    public override void Show<T>(UIInfo _uiInfo, T _param)
    {
        base.Show(_uiInfo);
        editorTileObject = _param as EditorTileObject;

        string showInfoStr = "";

        if(editorTileObject.tileData != null)
        {
            showInfoStr = $"Tile Data \n " +
                $"name : {editorTileObject.tileData.name}, " +
                $"index : {editorTileObject.tileData.index}, " +
                $"type : {editorTileObject.tileData.type}, " +
                $"Direction : {editorTileObject.tileData.direction}, " +
                $"batchIdx : {editorTileObject.tileData.batchIdx}, " +
                $"roadType : {editorTileObject.tileData.roadType}";
        }
        else
        {
            showInfoStr = "Tile Data is null";
        }

        DefaultSetting();
        infoText.text = showInfoStr;
    }
    public void ExitBtnClick()
    {
        ClearBtnClick();
        Managers.Instance.GetManager<UIManager>().HideUI(uiInfo);
    }
    public void ApplyBtnClick()
    {
        bool isEmpty = categoryUIDic.Values.Any(x => x.IsNotValue());


        TileData originData = editorTileObject.tileData;

        TileData newTileData = new TileDataBuilder().SetRoadType(categoryUIDic["roadtype"].GetValue())
            .SetDirection(categoryUIDic["direction"].GetValue())
            .SetBatchIdx(int.Parse(categoryUIDic["batchidx"].GetValue().ToString()))
            .SetName(categoryUIDic["name"].GetValue().ToString())
            .SetType(originData.type)
            .SetIndex(originData.index)
            .Build();


        editorTileObject.Load(newTileData);

        if (editorTileObject.tileData != null)
        {
            infoText.text = $"Tile Data \n " +
                $"name : {editorTileObject.tileData.name}, " +
                $"index : {editorTileObject.tileData.index}, " +
                $"type : {editorTileObject.tileData.type}, " +
                $"Direction : {editorTileObject.tileData.direction}, " +
                $"batchIdx : {editorTileObject.tileData.batchIdx}, " +
                $"roadType : {editorTileObject.tileData.roadType}";
        }
    }

    public void ClearBtnClick()
    {
        // input field�� ���� empty ó��
        foreach(var item in categoryUIDic) 
        {
            item.Value.ClearBtnClick();
        }
    }

    void DefaultSetting()
    {
        foreach (var item in categoryUIDic) 
        { 
            item.Value.DefaultSetting();
        }
    }
}
