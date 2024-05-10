using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using YhProj.Game.Map;
using YhProj.Game.UI;
using YhProj.Game.YhEditor;
using YhProj.Game;


public class MapToolUI : EditorUI
{
    [System.Serializable]
    public struct sMapToolBtn
    {
        public ButtonType btnType;
        public sButtonUI buttonUI;
        public Button Btn => buttonUI.btn;

        public TextMeshProUGUI Text => buttonUI.btnText;
    }
    [System.Serializable]
    public struct sMapTextInput
    {
        public InputType inputType;
        public sTextInputUI textInput;
        public TMP_InputField InputField => textInput.inputField;

        public TextMeshProUGUI Text => textInput.titleText;
    }
    [System.Serializable]
    public struct sMapTextDropDown
    {
        public DropDownType dropDownType;
        public sTextDropDownUI textDropDown;
        public TMP_Dropdown Dropdown => textDropDown.dropdown;
        
        public TextMeshProUGUI Text => textDropDown.titleText;
    }
    public enum InputType
    {
        row,
        col,
        stage,
        lv,
    }
    public enum DropDownType
    {
        stage,
    }

    [SerializeField]
    private List<sMapToolBtn> buttonList = new List<sMapToolBtn>();

    [SerializeField]
    private List<sMapTextInput> textInputList = new List<sMapTextInput>();

    [SerializeField]
    private List<sMapTextDropDown> textDropDownList = new List<sMapTextDropDown>();

    [SerializeField]
    private TextMeshProUGUI logText;

    [SerializeField]
    private StageData curStageData = null;

    public override void Show(UIInfo _uiInfo)
    {
        baseDataHandler = EditorManager.Instance.GetDataHandler<StageHandler>();

        foreach (var dropDown in textDropDownList)
        {
            UnityAction<int> valueCallback = null;

            switch (dropDown.dropDownType)
            {
                case DropDownType.stage:
                    valueCallback = OnDropdownValueChanged;
                    break;
            }

            if (valueCallback != null)
            {
                dropDown.Dropdown.onValueChanged.AddListener(valueCallback);
                dropDown.Text.text = dropDown.dropDownType.ToString();
            }
        }

        foreach (var btn in buttonList)
        {
            btn.Btn.onClick.RemoveAllListeners();

            UnityAction btnCallback = null;

            switch (btn.btnType)
            {
                case ButtonType.save:
                    btnCallback = SaveBtnClick;
                    break;
                case ButtonType.load:
                    btnCallback = LoadBtnClick;
                    break;
                case ButtonType.delete:
                    btnCallback = DeleteBtnClick;
                    break;
            }

            if (btnCallback != null)
            {
                btn.Btn.onClick.AddListener(btnCallback);
                btn.Text.text = btn.btnType.ToString();
            }
        }

        foreach (var text in textInputList)
        {
            text.InputField.text = "";
            text.Text.text = text.inputType.ToString();
        }

        logText.text = "Empty";

        TMP_Dropdown stageDropDown = textDropDownList.Find(x => x.dropDownType == DropDownType.stage).Dropdown;

        UpdateDropDown(stageDropDown);

        if (stageDropDown.options.Count > 0)
        {
            int idx = int.Parse(stageDropDown.options[stageDropDown.value].text);
            //StageHandler stageHandler = EditorManager.Instance.GetDataHandler<StageHandler>();

            StageData stageData = baseDataHandler.GetData<StageData>(idx);

            curStageData = stageData;

            for (int i = 0; i < textInputList.Count; i++)
            {
                textInputList[i].InputField.text = GetPropertyValue(curStageData, textInputList[i].inputType).ToString();
            }
        }
    }

    /// <summary>
    /// Stage Data에 표시되어져 있는 모든 tile data json으로 저장하는 메서드
    /// </summary>
    public void SaveBtnClick()
    {
        string text = "";

        if (curStageData == null)
        {
            text = "Save Fail!! StageData is null";
        }
        else
        {
            text = $"Save Complete!! Stage : {curStageData.stage}";

            // IDataHandler dataHandler = EditorManager.Instance.GetDataHandler<StageHandler>();

            EditorManager.Instance.Save(baseDataHandler, curStageData);
            TMP_Dropdown stageDropDown = textDropDownList.Find(x => x.dropDownType == DropDownType.stage).Dropdown;
            UpdateDropDown(stageDropDown);
        }

        DeleteBtnClick();
        curStageData = null;
        logText.text = text;
    }
    /// <summary>
    /// Stage data에 맞게끔 tileobject를 생성하는 메서드
    /// </summary>
    public void LoadBtnClick()
    {
        if (curStageData != null)
        {
            EditorManager.Instance.Delete(curStageData);
        }

        var list = GetEnumValues<InputType>();
        var dic = new Dictionary<InputType, TMP_InputField>();

        foreach(var item in list)
        {
            TMP_InputField inputField = textInputList.Find(x => x.inputType == item).InputField;

            dic.Add(item, inputField);
        }

        int row = GetInputFieldToType<int>(dic[InputType.row]);
        int col = GetInputFieldToType<int>(dic[InputType.col]);
        int stage = GetInputFieldToType<int>(dic[InputType.stage]);
        int lv = GetInputFieldToType<int>(dic[InputType.lv]);

        logText.text = "Stage : " + stage.ToString();

        // StageHandler stageHandler = EditorManager.Instance.GetDataHandler<StageHandler>();

        List<int> tileIdxList = baseDataHandler.GetData<StageData>(stage).tileIdxList;
        tileIdxList = tileIdxList == null ? new List<int>() : tileIdxList;
        int tileCount = row * col;

        for (int i = 0; i < tileCount; i++)
        {
            if (i > list.Count)
            {
                tileIdxList.Add(0);
            }
        }

        StageData stageData = new StageData(stage, lv, tileIdxList);

        curStageData = stageData;

        EditorManager.Instance.Create(curStageData);
    }
    /// <summary>
    /// Stage Data에 표시되어져 있는 모든 tile data 삭제하는 메서드
    /// </summary>
    public void DeleteBtnClick()
    {
        // Managers.Instance.GetManager<MapManager>().DeleteTile(curStageData.stage);
        logText.text = "Stage : Empty";
        curStageData = null;
        textInputList.ForEach(ui => ui.InputField.text = "");

        TMP_Dropdown stageDropDown = textDropDownList.Find(x => x.dropDownType == DropDownType.stage).Dropdown;
        // StageHandler stageHandler = EditorManager.Instance.GetDataHandler<StageHandler>();

        if (stageDropDown.options.Count > 0)
        {
            int idx = int.Parse(stageDropDown.options[stageDropDown.value].text);
            StageData stageData = baseDataHandler.GetData<StageData>(idx);

            curStageData = stageData;

            for (int i = 0; i < textInputList.Count; i++)
            {
                textInputList[i].InputField.text = GetPropertyValue(curStageData, textInputList[i].inputType).ToString();
            }
        }

    }

    /// <summary>
    /// Stage Data를 삭제함
    /// </summary>
    public void RemoveBtnClick()
    {
        TMP_InputField inputField = textInputList.Find(x => x.inputType == InputType.stage).InputField;

        int stage = GetInputFieldToType<int>(inputField);

        // StageHandler stageHandler = EditorManager.Instance.GetDataHandler<StageHandler>();

        if (baseDataHandler.ContainsData(stage))
        {
            StageData stageData = baseDataHandler.GetData<StageData>(stage);
            EditorManager.Instance.Delete(stageData);
            logText.text = "Delete Stage Success!!";
        }
        else
        {

        }
    }
    public void OnDropdownValueChanged(int _value)
    {
        TMP_Dropdown stageDropDown = textDropDownList.Find(x => x.dropDownType == DropDownType.stage).Dropdown;
        int idx = int.Parse(stageDropDown.options[_value].text);

        // StageHandler stageHandler = EditorManager.Instance.GetDataHandler<StageHandler>();

        StageData stageData = baseDataHandler.GetData<StageData>(idx);

        if (stageData != null)
        {
            curStageData = stageData;

            for (int i = 0; i < textInputList.Count; i++)
            {
                textInputList[i].InputField.text = GetPropertyValue(curStageData, textInputList[i].inputType).ToString();
            }
        }
        else
        {

        }
    }
    private object GetPropertyValue(StageData _stageData, InputType _inputType)
    {
        return _inputType switch
        {
            InputType.row => _stageData.row,
            InputType.col => _stageData.col,
            InputType.stage => _stageData.stage,
            InputType.lv => _stageData.lv,
            _ => null
        };
    }

    protected void UpdateDropDown(TMP_Dropdown _dropDown)
    {
        TMP_Dropdown stageDropDown = _dropDown;
        stageDropDown.ClearOptions();

        var stageList = baseDataHandler.GetDataList<StageData>();
        List<string> dropDownOptionList = stageList.Select(x => x.stage.ToString()).ToList();

        stageDropDown.AddOptions(dropDownOptionList);
    }

    public override void Hide()
    {
        base.Hide();
        curStageData = null;
    }
}
