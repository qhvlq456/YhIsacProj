using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using YhProj.Game.UI;
using YhProj.Game.Map;
using System.Linq;

namespace YhProj.Game.YhEditor
{
    public class EditorUI : BaseUI
    {
        public enum ButtonType
        {
            save,
            load,
            delete,
        }

        [Serializable]
        public struct sInputButtonUI
        {
            public TMP_InputField inputField;
            public Button button;
            public TextMeshProUGUI buttonText;
        }

        [Serializable]
        public struct sTextInputUI
        {
            public TextMeshProUGUI titleText;
            public TMP_InputField inputField;
        }

        [Serializable]
        public struct sButtonUI
        {
            public Button btn;
            public TextMeshProUGUI btnText;
        }

        [Serializable]
        public struct sTextDropDownUI
        {
            public TMP_Dropdown dropdown;
            public TextMeshProUGUI titleText;
        }

        // 후에 타입별로 클래스로 나누어 다시 짤것임
        protected T GetInputFieldToType<T>(TMP_InputField _inputField)
        {
            T ret = default(T);

            string inputText = _inputField.text;

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

        bool TryParseValue<T>(string _inputText, out T value)
        {
            value = default(T);

            if (System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).IsValid(_inputText))
            {
                value = (T)System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(_inputText);
                return true;
            }

            return false;
        }

        protected void UpdateDropDown(TMP_Dropdown _dropDown)
        {
            TMP_Dropdown stageDropDown = _dropDown;
            stageDropDown.ClearOptions();

            List<string> dropDownOptionList = Managers.Instance.GetManager<MapManager>().GetStageDataList().Select(x => x.stage.ToString()).ToList();
            stageDropDown.AddOptions(dropDownOptionList);
        }

        protected List<T> GetEnumValues<T>() 
        { 
            if(!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            List<T> ret = new List<T>();

            foreach(var value in Enum.GetValues(typeof(T))) 
            {
                ret.Add((T)value);
            }

            return ret;
        }
    }
}
