using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YhProj.Game.UI;
using YhProj.Game.Character;
using YhProj.Game.YhEditor;

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

    private CharacterObject characterObject;

}
