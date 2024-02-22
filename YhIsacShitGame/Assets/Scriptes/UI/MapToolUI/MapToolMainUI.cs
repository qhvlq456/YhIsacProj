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
    /// Stage Data�� ǥ�õǾ��� �ִ� ��� tile data json���� �����ϴ� �޼���
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
    /// Stage data�� �°Բ� tileobject�� �����ϴ� �޼��� // stage data �� �ʿ���
    /// </summary>
    public void LoadBtnClick() 
    {
        // ����/ ������ ������� ��
        // �ε� ���� �������� �����͸� ������ �־�� �ϴ� �͵� �Ǵ��ؾ� ��
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
    /// Stage Data�� ǥ�õǾ��� �ִ� ��� tile data �����ϴ� �޼���
    /// </summary>
    public void DeleteBtnClick() 
    {
        stageText.text = "Stage : Empty";
        currentStageData = null;

        Managers.Instance.GetManager<MapManager>().DeleteTile();
    }

    // �Ŀ� Ÿ�Ժ��� Ŭ������ ������ �ٽ� ©����
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
