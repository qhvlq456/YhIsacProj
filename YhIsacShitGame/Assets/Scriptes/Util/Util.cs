using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using UnityEngine;
using System;

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
    public static void CreateJsonFile(string _createPath, string _fileName, object _jsonData)
    {
        string jsonData = DataToJson(_jsonData);
        string filePath = string.Format("{0}/{1}/{2}.json", Application.dataPath, _createPath, _fileName);

        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();
            }

            Debug.Log($"JSON file created successfully at: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create JSON file. Exception: {e.Message}");
        }

        // FileStream fileStream = new FileStream(Path.Combine(Application.dataPath + _createPath + _fileName), FileMode.Create);
        //byte[] data = Encoding.UTF8.GetBytes(jsonData);
        //fileStream.Write(data, 0, data.Length);
        //fileStream.Close();
    }
    public static T LoadJson<T>(string _loadPath, string _fileName)
    {
        string filePath = string.Format("{0}/{1}/{2}", Application.dataPath, _loadPath, _fileName);

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
            Debug.LogWarningFormat("Utile LoadJson Warning \n filePath : {0}, _loadPath : {1}, _fileName : {2}}", filePath, _loadPath, _fileName);
            return default;
        }
    }

    public static List<T> LoadJsonArray<T>(string _loadPath, string _fileName)
    {
        string filePath = string.Format("{0}/{1}/{2}", Application.dataPath, _loadPath, _fileName);

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
            Debug.LogWarningFormat("Util LoadJsonArray Warning \n filePath : {0}, _loadPath : {1}, _fileName : {2}", filePath, _loadPath, _fileName);
            return new List<T>(); // 또는 예외 처리를 추가하여 반환값을 선택할 수 있습니다.
        }
    }
    #endregion

    #region Resource Util
    public static T InstantiateResource<T>(string _path) where T : UnityEngine.Object
    {
        GameObject resObj = Resources.Load<GameObject>(_path);

        if(resObj == null)
        {
            Debug.LogWarningFormat("Util GetResource resobj Warning \n resobj : {0}, path : {1}", resObj, _path);
            return null;
        }

        T copyObj = UnityEngine.Object.Instantiate(resObj).GetComponent<T>();

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
    // 후에 find 이름을 찾는 법도 만들어야 할 듯?
    public static T AttachObj<T>(string _name = null) where T :  Component
    {
        T ret = null;

        string name = _name;

        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject _target = GameObject.Find(name);

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

    public static T AttachObj<T>(GameObject _go, string _name = null) where T : Component
    {
        string name = _name;

        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        _go = GameObject.Find(name);

        if (_go == null)
        {
            GameObject container = new GameObject(name);
            _go = container;
        }

        T component = _go.GetComponent<T>();

        if (component == null)
        {
            component = _go.AddComponent<T>();
        }

        return component;
    }

    #endregion Attach Obj
}
