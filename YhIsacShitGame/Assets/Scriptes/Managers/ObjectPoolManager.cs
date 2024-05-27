using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YhProj.Game
{
    //public enum 
    public class ObjectPooling
    {
        private readonly string path;
        protected Transform root;

        public ObjectPooling(Transform _parent, string _path)
        {
            root = _parent;
            path = _path;
        }
        public virtual Transform Pooling(string _objName, string _subPath)
        {
            Transform ret = null;
            // start 후에 이 부분만 partial하여 resmanager를 만들어서 관리
            string combinePath = string.Format("{0}/{1}/{2}", path, _subPath, _objName);
            ret = Create(combinePath);

            return ret;
        }
        public virtual Transform Pooling(string _objName)
        {
            Transform ret = null;
            // start 후에 이 부분만 partial하여 resmanager를 만들어서 관리
            string combinePath = string.Format("{0}/{1}", path, _objName);
            ret = Create(combinePath);

            return ret;
        }
        private Transform Create(string _combinePath)
        {
            Transform ret = null;

            if (root.childCount == 0)
            {
                // active ture or false 는 portrait 때문에 한건데 일단 보류
                ret = GameUtil.InstantiateResource<Transform>(_combinePath);
                ret.parent = root;
                ret.gameObject.SetActive(false);
            }
            else
            {
                ret = root.GetChild(0);
            }

            ret.gameObject.SetActive(true);

            return ret;
        }
        public virtual void Retrieve(Transform _trf)
        {
            _trf.parent = root;
            _trf.localPosition = Vector3.zero;
            _trf.localRotation = Quaternion.identity;
            _trf.localScale = Vector3.one;
            _trf.gameObject.SetActive(true);
        }
    }

    public class ObjectPoolManager : BaseManager
    {
        private readonly Dictionary<BaseType, string> poolingPathMap = new Dictionary<BaseType, string>()
        {
            { BaseType.tile, "Map/Tool/"},
            { BaseType.character, "Character/"},
            { BaseType.build, "Build/"},
            { BaseType.accessory, "Accessory/"},
            { BaseType.item, "Item/"},
        };
        // object pool 상위 부모 자식들의 갯수에 따라 생성 가능과 회수를 결정한다
        private Dictionary<BaseType, ObjectPooling> parentPoolingMap = new Dictionary<BaseType, ObjectPooling>();
        
        public override void Load()
        {
            root = GameUtil.AttachObj<Transform>("ObjectPoolRoot");
            root.position = Vector3.back * 100f;

            foreach (var pooling in poolingPathMap)
            {
                string objName = pooling.Key.GetType().Name;
                GameObject go = new GameObject(objName);
                go.transform.parent = root;
                go.transform.localPosition = Vector3.zero;

                parentPoolingMap.Add(BaseType.tile, new ObjectPooling(go.transform, pooling.Value));
            }
        }

        public override void Dispose()
        {

        }

        public override void Update()
        {

        }
        
        public Transform Pooling(BaseType _baseType, string _resName, string _subPath)
        {
            Transform ret = null;
            ret = GetPooling(_baseType).Pooling(_resName, _subPath);

            return ret;
        }

        public Transform Pooling(BaseType _baseType, string _resName)
        {
            Transform ret = null;
            ret = GetPooling(_baseType).Pooling(_resName);

            return ret;
        }
        private ObjectPooling GetPooling(BaseType _baseType)
        {
            ObjectPooling ret = null;

            if (parentPoolingMap.ContainsKey(_baseType))
            {
                ret = parentPoolingMap[_baseType];
            }
            else
            {
                string path = poolingPathMap[_baseType];

                string objName = _baseType.GetType().Name;
                GameObject go = new GameObject(objName);
                go.transform.parent = root;
                go.transform.localPosition = Vector3.zero;

                ObjectPooling objectPooling = new ObjectPooling(go.transform, path);

                parentPoolingMap.Add(_baseType, objectPooling);

                ret = objectPooling;
            }

            return ret;
        }
        /// <summary>
        /// 회수 함수 
        /// </summary>
        public void Retrieve(BaseType _baseType, Transform _trf)
        {
            if (parentPoolingMap.ContainsKey(_baseType))
            {
                parentPoolingMap[_baseType].Retrieve(_trf);
            }
        }
    }
}
