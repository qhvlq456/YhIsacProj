using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapToolMainUI : MainUI
{
    [Serializable]
    public struct sTextInputUI
    {
        public InputType inputType;
        public TextMeshProUGUI titleText;
        public TMP_InputField inputField;
    }

    [Serializable]
    public struct sButtonUI
    {
        public ButtonType buttonType;
        public Button btn;
        public TextMeshProUGUI btnText;
    }

    [Serializable]
    public struct sTextDropDownUI
    {
        public DropDownType dropDownType;
        public TMP_Dropdown dropdown;
        public TextMeshProUGUI titleText;
    }
    public enum ButtonType
    {
        save,
        load,
        delete,
    }
    public enum InputType
    {
        row,
        col,
        stage,
        lv,
        tileSize,
        xOffset,
        zOffset
    }
    public enum DropDownType
    {
        stage,
    }

    [SerializeField]
    private List<sButtonUI> buttonList = new List<sButtonUI>();

    [SerializeField]
    private List<sTextInputUI> textInputList = new List<sTextInputUI>();

    [SerializeField]
    private List<sTextDropDownUI> textDropDownList = new List<sTextDropDownUI>();

    [SerializeField]
    private TextMeshProUGUI logText;

    [SerializeField]
    private StageData currentStageData = null;

    private void Awake()
    {
        foreach(var dropDown in textDropDownList) 
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
                dropDown.dropdown.onValueChanged.AddListener(valueCallback);
                dropDown.titleText.text = dropDown.dropDownType.ToString();
            }
        }

        foreach (var btn in  buttonList)
        {
            btn.btn.onClick.RemoveAllListeners();

            UnityAction btnCallback = null;

            switch (btn.buttonType)
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
                btn.btn.onClick.AddListener(btnCallback);
                btn.btnText.text = btn.buttonType.ToString();
            }
        }

        foreach(var text in textInputList)
        {
            text.inputField.text = "";
            text.titleText.text = text.inputType.ToString();
        }

        logText.text = "Empty";
    }
    public override void Show(UIInfo _uiInfo)
    {
        base.Show(_uiInfo);

        TMP_Dropdown stageDropDown = textDropDownList.Find(x => x.dropDownType == DropDownType.stage).dropdown;
        stageDropDown.ClearOptions();

        List<string> dropDownOptionList = Managers.Instance.GetManager<MapManager>().GetStageDataList().Select(x => x.stage.ToString()).ToList();
        stageDropDown.AddOptions(dropDownOptionList);
    }

    /// <summary>
    /// Stage Data에 표시되어져 있는 모든 tile data json으로 저장하는 메서드
    /// </summary>
    public void SaveBtnClick() 
    {
        string text = "";

        if(currentStageData == null)
        {
            text = "Save Fail!! StageData is null";
        }
        else
        {
            text = $"Save Complete!! Stage : {currentStageData.stage}";
            Managers.Instance.GetManager<MapManager>().SaveMapTool(currentStageData);

            TMP_Dropdown stageDropDown = textDropDownList.Find(x => x.dropDownType == DropDownType.stage).dropdown;
            stageDropDown.ClearOptions();

            List<string> dropDownOptionList = Managers.Instance.GetManager<MapManager>().GetStageDataList().Select(x => x.stage.ToString()).ToList();
            stageDropDown.AddOptions(dropDownOptionList);

            textInputList.ForEach(ui => ui.inputField.text = "");
        }

        logText.text = text;
    }
    /// <summary>
    /// Stage data에 맞게끔 tileobject를 생성하는 메서드
    /// </summary>
    public void LoadBtnClick() 
    {
        int row = GetInputFieldToType<int>(InputType.row);
        int col = GetInputFieldToType<int>(InputType.col);
        int stage = GetInputFieldToType<int>(InputType.stage);
        int lv = GetInputFieldToType<int>(InputType.lv);

        float tileSize = GetInputFieldToType<float>(InputType.tileSize);
        float xOffset = GetInputFieldToType<float>(InputType.xOffset);
        float zOffset = GetInputFieldToType<float>(InputType.zOffset);

        logText.text = "Stage : " + stage.ToString();


        // 이 부분이 문제인듯? 또 딜레마네 ,, 아무리 생각을해도 토글이 필요 할 듯?
        // 아님 json에 타일 데이터가 존재한다면 그 값을 가져와서 사용하면 될거 같긴 함
        // 근데 애초에 두개 분리 시켜서 한 것이 더 좋아보이기도 함
        TileData[,] copyTileArr = Managers.Instance.GetManager<MapManager>().GetStateByTileArr(stage);

        StageData stageData = new StageData(stage, lv, copyTileArr == null ? new TileData[row,col] : copyTileArr);

        stageData.tileSize = tileSize;
        stageData.xOffset = xOffset;
        stageData.zOffset = zOffset;

        currentStageData = stageData;

        Managers.Instance.GetManager<MapManager>().LoadTile(currentStageData);
    }
    /// <summary>
    /// Stage Data에 표시되어져 있는 모든 tile data 삭제하는 메서드
    /// </summary>
    public void DeleteBtnClick() 
    {
        logText.text = "Stage : Empty";
        currentStageData = null;

        Managers.Instance.GetManager<MapManager>().DeleteTile();
    }
    
    /// <summary>
    /// Stage Data를 삭제함
    /// </summary>
    public void RemoveBtnClick()
    {
        int stage = GetInputFieldToType<int>(InputType.stage);

        if(Managers.Instance.GetManager<MapManager>().IsConstainsStage(stage))
        {
            Managers.Instance.GetManager<MapManager>().DeleteMapTool(stage);
            logText.text = "Delete Stage Success!!";
        }
        else
        {

        }
    }
    public void OnDropdownValueChanged(int _value)
    {
        StageData stageData = Managers.Instance.GetManager<MapManager>().GetStageData(_value);

        if(stageData != null)
        {
            currentStageData = stageData;

            for(int i = 0; i < textInputList.Count; i++)
            {
                textInputList[i].inputField.text = GetPropertyValue(currentStageData, textInputList[i].inputType).ToString();
            }
        }
        else
        {

        }
    }
    object GetPropertyValue(StageData stageData, InputType inputType)
    {
        return inputType switch
        {
            InputType.row => stageData.Row,
            InputType.col => stageData.Col,
            InputType.stage => stageData.stage,
            InputType.lv => stageData.lv,
            InputType.tileSize => stageData.tileSize,
            InputType.xOffset => stageData.xOffset,
            InputType.zOffset => stageData.zOffset,
            _ => null
        };
    }

    // 후에 타입별로 클래스로 나누어 다시 짤것임
    T GetInputFieldToType<T>(InputType _inputType)
    {
        T ret = default(T);

        string inputText = textInputList.Find(x => x.inputType == _inputType).inputField.text;

        if (TryParseValue(inputText, out T value))
        {
            ret = value;
        }
        else
        {
            // log
        }

        return ret;
    }

    bool TryParseValue<T>(string _inputText,  out T value)
    {
        value = default(T);

        if (System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).IsValid(_inputText))
        {
            value = (T)System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(_inputText);
            return true;
        }

        return false;
    }
}
