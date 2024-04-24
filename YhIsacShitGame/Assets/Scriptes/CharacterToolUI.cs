using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YhProj.Game.UI;
using YhProj.Game.Character;
using YhProj.Game.YhEditor;
using UnityEngine.Events;
using YhProj.Game.Map;

public class CharacterToolUI : EditorUI
{
    [System.Serializable]
    public struct sCharToolBtn
    {
        public ButtonType btnType;
        public sButtonUI buttonUI;
        public Button Btn => buttonUI.btn;

        public TextMeshProUGUI Text => buttonUI.btnText;
    }
    [System.Serializable]
    public struct sCharTextInput
    {
        public InputType inputType;
        public sTextInputUI textInput;
        public TMP_InputField InputField => textInput.inputField;

        public TextMeshProUGUI Text => textInput.titleText;
    }
    [System.Serializable]
    public struct sCharTextDropDown
    {
        public DropDownType dropDownType;
        public sTextDropDownUI textDropDown;
        public TMP_Dropdown Dropdown => textDropDown.dropdown;

        public TextMeshProUGUI Text => textDropDown.titleText;
    }
    [System.Serializable]
    public struct sCharInputButton
    {
        public InputType inputType;
        public sInputButtonUI inputBtn;

        public Button Button => inputBtn.button;
        public TextMeshProUGUI ButtonText => inputBtn.buttonText;
        public TMP_InputField InputField => inputBtn.inputField;

    }

    public enum InputType
    {
        index,
        name,
        resName,
        health,
        armor,
        power,
        range,
        moveSpeed
    }
    public enum DropDownType
    {
        baseType,
        elementType,
        attributeType,
    }

    // index 기준으로 dropdonwlist
    [SerializeField]
    private List<sCharToolBtn> charToolBtnList = new List<sCharToolBtn>();

    [SerializeField]
    private List<sCharTextDropDown> charTextDropDownList = new List<sCharTextDropDown>();

    [SerializeField]
    private List<sCharTextInput> charTextInputList = new List<sCharTextInput>();

    [SerializeField]
    private List<sCharInputButton> charInputBtnList = new List<sCharInputButton>();


    private CharacterData curCharData = null;

    public override void Show(UIInfo _uiInfo)
    {
        baseDataHandler = EditorManager.Instance.GetDataHandler<CharDataHandler>();

        foreach (var btn in charToolBtnList)
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

        base.Show(_uiInfo);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void SaveBtnClick()
    {
        EditorManager.Instance.Save(baseDataHandler, curCharData);
    }
    public void LoadBtnClick()
    {
        if (curCharData != null)
        {
            EditorManager.Instance.Delete(curCharData);
        }

        var list = GetEnumValues<InputType>();
        var dic = new Dictionary<InputType, TMP_InputField>();

        foreach (var item in list)
        {
            TMP_InputField inputField = charTextInputList.Find(x => x.inputType == item).InputField;

            dic.Add(item, inputField);
        }

        int index = GetInputFieldToType<int>(dic[InputType.index]);
        string name = GetInputFieldToType<string>(dic[InputType.name]);
        int health = GetInputFieldToType<int>(dic[InputType.health]);
        int armor = GetInputFieldToType<int>(dic[InputType.armor]);
        int power = GetInputFieldToType<int>(dic[InputType.power]);
        int range = GetInputFieldToType<int>(dic[InputType.range]);
        int moveSpeed = GetInputFieldToType<int>(dic[InputType.moveSpeed]);

        CharacterData characterData = baseDataHandler.GetData<CharacterData>(index);
        characterData = characterData == null ? new CharacterData() : characterData;
        //logText.text = "Stage : " + stage.ToString();

        curCharData = characterData;

        EditorManager.Instance.Create(curCharData);
    }
    public void DeleteBtnClick()
    {
        //if (baseDataHandler.ContainsData(stage))
        //{
        //    StageData stageData = baseDataHandler.GetData<StageData>(stage);
        //    EditorManager.Instance.Delete(stageData);
        //    logText.text = "Delete Stage Success!!";
        //}
        //else
        //{

        //}
    }
    public void RemoveBtnClick()
    {
        TMP_InputField inputField = textInputList.Find(x => x.inputType == InputType.index).InputField;

        int stage = GetInputFieldToType<int>(inputField);

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
}
