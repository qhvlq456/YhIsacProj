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
    private StageData currentStageData = null;

    public override void Show(UIInfo _uiInfo)
    {
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
            StageData stageData = EditorManager.Instance.GetDataHandler<StageData>().GetData(idx);

            currentStageData = stageData;

            for (int i = 0; i < textInputList.Count; i++)
            {
                textInputList[i].InputField.text = GetPropertyValue(currentStageData, textInputList[i].inputType).ToString();
            }
        }
    }

    /// <summary>
    /// Stage Data에 표시되어져 있는 모든 tile data json으로 저장하는 메서드
    /// </summary>
    public void SaveBtnClick()
    {
        string text = "";

        if (currentStageData == null)
        {
            text = "Save Fail!! StageData is null";
        }
        else
        {
            text = $"Save Complete!! Stage : {currentStageData.stage}";

            EditorManager.Instance.Save(currentStageData);
            TMP_Dropdown stageDropDown = textDropDownList.Find(x => x.dropDownType == DropDownType.stage).Dropdown;
            UpdateDropDown(stageDropDown);
        }

        DeleteBtnClick();
        currentStageData = null;
        logText.text = text;
    }
    /// <summary>
    /// Stage data에 맞게끔 tileobject를 생성하는 메서드
    /// </summary>
    public void LoadBtnClick()
    {
        if (currentStageData != null)
        {
            EditorManager.Instance.Delete(currentStageData);
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

        List<int> tileIdxList = EditorManager.Instance.GetDataHandler<StageData>().GetData(stage).tileIdxList;
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

        currentStageData = stageData;

        EditorManager.Instance.Create(currentStageData);
    }
    /// <summary>
    /// Stage Data에 표시되어져 있는 모든 tile data 삭제하는 메서드
    /// </summary>
    public void DeleteBtnClick()
    {
        // Managers.Instance.GetManager<MapManager>().DeleteTile(currentStageData.stage);
        logText.text = "Stage : Empty";
        currentStageData = null;
        textInputList.ForEach(ui => ui.InputField.text = "");

        TMP_Dropdown stageDropDown = textDropDownList.Find(x => x.dropDownType == DropDownType.stage).Dropdown;

        if (stageDropDown.options.Count > 0)
        {
            int idx = int.Parse(stageDropDown.options[stageDropDown.value].text);
            StageData stageData = EditorManager.Instance.GetDataHandler<StageData>().GetData(idx);

            currentStageData = stageData;

            for (int i = 0; i < textInputList.Count; i++)
            {
                textInputList[i].InputField.text = GetPropertyValue(currentStageData, textInputList[i].inputType).ToString();
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

        StageHandler stageHandler = EditorManager.Instance.GetDataHandler<StageData>() as StageHandler;

        if (stageHandler.ContainsData(stage))
        {
            StageData stageData = stageHandler.GetData(stage);
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

        StageData stageData = EditorManager.Instance.GetDataHandler<StageData>().GetData(idx);

        if (stageData != null)
        {
            currentStageData = stageData;

            for (int i = 0; i < textInputList.Count; i++)
            {
                textInputList[i].InputField.text = GetPropertyValue(currentStageData, textInputList[i].inputType).ToString();
            }
        }
        else
        {

        }
    }
    object GetPropertyValue(StageData _stageData, InputType _inputType)
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
}
