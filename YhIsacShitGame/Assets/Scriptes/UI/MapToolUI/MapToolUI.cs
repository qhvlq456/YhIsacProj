using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using YhProj;
using TMPro;
using System.Linq;
using YhProj.Game.Map;
using YhProj.Game.UI;
using YhProj.Game;

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
    // 현재 클릭해서 UI를 표시되게한 오브젝트
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

            MapToolCategoryUI mapToolCategoryUI = GameUtil.InstantiateResource<MapToolCategoryUI>(mapToolCategoryPath);
            mapToolCategoryUI.transform.SetParent(contentTrf, false);
            mapToolCategoryUI.Set(field);

            categoryUIDic.Add(field.Name.ToLower(), mapToolCategoryUI);
        }
    }

    public override void Show<T>(UIInfo _uiInfo, T _param)
    {
        base.Show(_uiInfo);
        editorTileObject = _param as EditorTileObject;

        InfoText();
    }
    public void ExitBtnClick()
    {
        ClearBtnClick();
        Managers.Instance.GetManager<UIManager>().HideUI(uiInfo);
    }
    public void ApplyBtnClick()
    {
        bool isEmpty = categoryUIDic.Values.Any(x => x.IsNotValue());

        if(isEmpty) 
        {
            Debug.LogError($"is empty");
        }
        else
        {
            TileData originData = editorTileObject.tileData;

            TileData newTileData = new TileDataBuilder().SetRoadType(categoryUIDic["elementtype"].GetValue())
                .SetDirection(categoryUIDic["direction"].GetValue())
                .SetBatchIdx(int.Parse(categoryUIDic["batchidx"].GetValue().ToString()))
                .SetName(categoryUIDic["name"].GetValue().ToString())
                .SetType(originData.type)
                .SetIndex(originData.index)
                .Build();


            editorTileObject.Create(newTileData);
        }

        InfoText();
    }

    public void ClearBtnClick()
    {
        // input field들 전부 empty 처리
        foreach(var item in categoryUIDic) 
        {
            item.Value.ClearBtnClick();
        }
    }

    void InfoText()
    {
        string showInfoStr = "";

        if (editorTileObject.tileData != null)
        {
            showInfoStr = $"Tile Data \n " +
                $"name : {editorTileObject.tileData.name}, " +
                $"index : {editorTileObject.tileData.index}, " +
                $"type : {editorTileObject.tileData.type}, " +
                $"Direction : {editorTileObject.tileData.direction}, " +
                $"batchIdx : {editorTileObject.tileData.batchIdx}, " +
                $"roadType : {editorTileObject.tileData.elementType}";
        }
        else
        {
            showInfoStr = "Tile Data is null";
        }

        infoText.text = showInfoStr;
    }
}
