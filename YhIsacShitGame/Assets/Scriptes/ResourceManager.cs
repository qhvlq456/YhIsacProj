using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YhProj.Game.Resource
{
    public class ResourceManager : BaseManager
    {
        //private Dictionary<string, AudioClip> audioResMap = new Dictionary<string, AudioClip>();
        //private Dictionary<string, Texture2D> texureResMap = new Dictionary<string, Texture2D>();
        //private Dictionary<string, Animation> animResMap = new Dictionary<string, Animation>();
        //private Dictionary<string, GameObject> objResMap = new Dictionary<string, GameObject>();
        //private Dictionary<string, Material> matResMap = new Dictionary<string, Material>();

        private Dictionary<System.Type, object> resourceDicts = new Dictionary<System.Type, object>();

        public T GetResByString<T>(string _name) where T : Object
        {
            System.Type type = typeof(T);

            if (resourceDicts.ContainsKey(type))
            {
                var dictionary = resourceDicts[type] as ResourceDictionary<T>;
                return dictionary.GetResource(_name);
            }

            Debug.LogWarning($"Resource dictionary for type '{type}' not found.");
            return null;
        }

        public void AddResource<T>(string _name, T _resource) where T : Object
        {
            System.Type type = typeof(T);

            if (resourceDicts.ContainsKey(type))
            {
                var dictionary = resourceDicts[type] as ResourceDictionary<T>;
                dictionary.AddResource(_name, _resource);
            }
            else
            {
                Debug.LogWarning($"Resource dictionary for type '{type}' not found.");
            }
        }

        public override void Dispose()
        {
            // 리소스 해제 로직을 여기에 추가합니다.
        }

        public override void Load()
        {
            // 리소스 로드 로직을 여기에 추가합니다.
        }

        public override void Update()
        {
            // 업데이트 로직을 여기에 추가합니다.
        }

        private void AddResourceDictionary<T>() where T : Object
        {
            System.Type type = typeof(T);
            if (!resourceDicts.ContainsKey(type))
            {
                resourceDicts[type] = new ResourceDictionary<T>();
            }
            else
            {
                Debug.LogWarning($"Resource dictionary for type '{type}' already exists.");
            }
        }
    }

    public class ResourceDictionary<T> where T : Object
    {
        private Dictionary<string, T> resourceMap = new Dictionary<string, T>();

        public void AddResource(string _name, T _resource)
        {
            if (!resourceMap.ContainsKey(_name))
            {
                resourceMap.Add(_name, _resource);
            }
            else
            {
                Debug.LogWarning($"Resource with name '{_name}' already exists.");
            }
        }

        public T GetResource(string _name)
        {
            if (resourceMap.ContainsKey(_name))
            {
                return resourceMap[_name];
            }
            else
            {
                Debug.LogWarning($"Resource with name '{_name}' not found.");
                return null;
            }
        }
    }
}

