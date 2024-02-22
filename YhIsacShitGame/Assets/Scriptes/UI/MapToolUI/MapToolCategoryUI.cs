using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static YhProj.Define;
using System;
using System.Reflection;

public class MapToolCategoryUI : MonoBehaviour
{
    public FieldInfo fieldInfo;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TMP_Dropdown dropDown;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Button clearBtn;

    public object GetValue()
    {
        object ret = null;

        if (dropDown.gameObject.activeSelf)
        {
            ret = dropDown.options[dropDown.value].text;
        }

        if (inputField.gameObject.activeSelf)
        {
            ret = inputField.text;
        }

        return ret;
    }

    public void Set(FieldInfo _fieldInfo)
    {
        clearBtn.onClick.RemoveAllListeners();
        dropDown.onValueChanged.RemoveAllListeners();
        dropDown.options.Clear();

        clearBtn.onClick.AddListener(ClearBtnClick);

        fieldInfo = _fieldInfo;
        titleText.text = _fieldInfo.Name;

        Type type = _fieldInfo.FieldType;
        bool isDropDown = true;

        if(type ==  typeof(Direction) || type == typeof(BaseType) 
           || type == typeof(RoadType) || type == typeof(bool)) 
        {
            isDropDown = true;
        }
        else
        {
            isDropDown = false;
        }

        if (isDropDown)
        {
            List<string> list = type.GetDropdownOptions();

            dropDown.AddOptions(list);
            dropDown.gameObject.SetActive(true);
            inputField.gameObject.SetActive(false);
        }
        else
        {
            dropDown.gameObject.SetActive(false);
            inputField.gameObject.SetActive(true);
        }
    }

    public void ClearBtnClick()
    {
        if(dropDown.gameObject.activeSelf)
        {
            dropDown.value = 0;
        }

        if (inputField.gameObject.activeSelf)
        {
            inputField.text = "";
        }
    }
    
}
