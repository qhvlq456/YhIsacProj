using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MapToolMainUI : MainUI
{
    [Serializable]
    public struct sTextInputUI
    {
        public TextInputType textInputType;
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
    public enum ButtonType
    {
        save,
        load,
        delete,
    }
    public enum TextInputType
    {
        row,
        col,
        stage,
        lv,
        tileSize,
        xOffset,
        zOffset
    }

    public List<sButtonUI> buttonList = new List<sButtonUI>();
    public List<sTextInputUI> textInputList = new List<sTextInputUI>();

    [SerializeField]
    private Toggle editorToggle;

    [SerializeField]
    private TextMeshProUGUI stageText;

    [SerializeField]
    private StageData currentStageData = null;

    private void Awake()
    {
        foreach(var btn in  buttonList)
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
            text.titleText.text = text.textInputType.ToString();
        }

        stageText.text = "Empty";
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
            // inputfield is null
        }

        stageText.text = text;
    }
    /// <summary>
    /// Stage data에 맞게끔 tileobject를 생성하는 메서드 // stage data 가 필요함
    /// </summary>
    public void LoadBtnClick() 
    {
        // 생성/ 편집을 나누어야 함
        // 로드 이후 스테이지 데이터를 가지고 있어야 하는 것도 판단해야 함
        int row = GetInputFieldToType<int>(TextInputType.row);
        int col = GetInputFieldToType<int>(TextInputType.col);
        int stage = GetInputFieldToType<int>(TextInputType.stage);
        int lv = GetInputFieldToType<int>(TextInputType.lv);

        float tileSize = GetInputFieldToType<float>(TextInputType.tileSize);
        float xOffset = GetInputFieldToType<float>(TextInputType.xOffset);
        float zOffset = GetInputFieldToType<float>(TextInputType.zOffset);

        stageText.text = "Stage : " + stage.ToString();

        StageData stageData = new StageData(stage, lv, new TileData[row, col]);
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
        stageText.text = "Stage : Empty";
        currentStageData = null;

        Managers.Instance.GetManager<MapManager>().DeleteTile();
    }

    // 후에 타입별로 클래스로 나누어 다시 짤것임
    T GetInputFieldToType<T>(TextInputType _textInputType)
    {
        T ret = default(T);

        string inputText = textInputList.Find(x => x.textInputType == _textInputType).inputField.text;

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
