using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public static class Util
{
    #region Json Util
    static T JsonToData<T>(string _json)
    {
        return JsonConvert.DeserializeObject<T>(_json);
    }
    static string DataToJson(object _obj)
    {
        return JsonConvert.SerializeObject(_obj);
    }
    public static void CreateJsonFile(string _createPath, string _fileName, string _jsonData)
    {
        FileStream fileStream = new FileStream(Path.Combine(Application.dataPath + _createPath + _fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(_jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    public static T LoadJson<T>(string _loadPath, string _fileName)
    {
        string filePath = Path.Combine(Application.dataPath + _loadPath, _fileName);

        if(File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(Path.Combine(Application.dataPath + _loadPath + _fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonToData<T>(jsonData);
        }
        else
        {
            Debug.LogWarningFormat("Uile LoadJson Warning \n filePath : {0}, _loadPath : {1}, _fileName : {2}}", filePath, _loadPath, _fileName);
            return default;
        }
    }

    public static List<T> LoadJsonArray<T>(string _loadPath, string _fileName)
    {
        string filePath = Path.Combine(Application.dataPath + _loadPath, _fileName);

        if (File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonToData<List<T>>(jsonData);
        }
        else
        {
            Debug.LogWarningFormat("Uile LoadJsonArray Warning \n filePath : {0}, _loadPath : {1}, _fileName : {2}}", filePath, _loadPath, _fileName);
            return null; // 또는 예외 처리를 추가하여 반환값을 선택할 수 있습니다.
        }
    }
    #endregion

    #region Resource Util
    public static T InstantiateResource<T>(string _path) where T : Object
    {
        T resObj = Resources.Load<T>(_path);

        if(resObj == null)
        {
            Debug.LogWarningFormat("Uile GetResource resobj Warning \n resobj : {0}, path : {1}}", resObj, _path);
            return null;
        }

        T copyObj = Object.Instantiate(resObj);

        return copyObj;
    }
    #endregion

    #region Load BundleData
    public static void LoadBunlde()
    {
        // 정의 필요
    }
    #endregion

    #region UI Util
    public static T FindChild<T>(GameObject _go, string _name = null, bool _isRecursive = false) where T : UnityEngine.Object
    {
        if(_go == null)
        {
            return null;
        }

        if(!_isRecursive)
        {
            for(int i = 0; i < _go.transform.childCount; i++)
            {
                Transform child = _go.transform.GetChild(i);

                if(string.IsNullOrEmpty(child.name) || child.name == _name)
                {
                    T component = child.GetComponent<T>();

                    return component;
                }
            }
        }
        else
        {
            foreach (T component in _go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(_name) || component.name == _name)
                {
                    return component;
                }
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject _go, string name = null, bool _recursive = false)
    {
        Transform trf = FindChild<Transform>(_go, name, _recursive);

        if (trf == null)
        {
            return null;
        }

        return trf.gameObject;

    }
    #endregion

    #region Attach Obj
    public static T AttachObj<T>(GameObject _target, string _name = null) where T :  Component
    {
        T ret = null;

        string name = _name;

        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        _target = GameObject.Find(name);

        if (_target == null)
        {
            GameObject container = new GameObject(name);
            _target = container;
        }

        ret = _target.GetComponent<T>();

        if (ret == null)
        {
            ret = _target.AddComponent<T>();
        }

        return ret;
    }
    #endregion Attach Obj
}
