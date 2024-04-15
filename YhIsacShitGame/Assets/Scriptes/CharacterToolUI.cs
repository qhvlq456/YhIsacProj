using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YhProj.Game.UI;
using YhProj.Game.Character;
using YhProj.Game.YhEditor;
using System.Net.NetworkInformation;

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

    private CharacterObject characterObject;


    [SerializeField]
    private List<sCharToolBtn> charToolBtnList = new List<sCharToolBtn>();

    [SerializeField]
    private List<sCharTextDropDown> charTextDropDownList = new List<sCharTextDropDown>();

    [SerializeField]
    private List<sCharTextInput> charTextInputList = new List<sCharTextInput>();

    [SerializeField]
    private List<sCharInputButton> charInputBtnList = new List<sCharInputButton>();
}
