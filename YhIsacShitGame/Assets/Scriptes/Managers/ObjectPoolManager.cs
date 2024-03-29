using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YhProj.Game
{
    public class ObjectPoolManager : BaseManager
    {
        // object pool의 최상단 root 부모
        private Transform root;
        // object pool 상위 부모 자식들의 갯수에 따라 생성 가능과 회수를 결정한다
        private Dictionary<Define.BaseType, Transform> parentTrfDic = new Dictionary<Define.BaseType, Transform>();
        
        public override void Load(Define.GameMode _gameMode)
        {
            root = GameUtil.AttachObj<Transform>("ObjectPoolRoot");
            root.position = Vector3.back * 100f;

            for (int i = 0; i < (int)Define.BaseType.COUNT; i++)
            {
                Define.BaseType baseType = (Define.BaseType)i;
                string objName = baseType.ToString();

                GameObject go = new GameObject(objName);
                go.transform.parent = root;
                go.transform.localPosition = Vector3.zero;

                parentTrfDic.Add(baseType, go.transform);
            }

            switch (_gameMode)
            {
                case Define.GameMode.EDITOR:
                    break;
                case Define.GameMode.TEST:
                    break;
                case Define.GameMode.MAPTOOL:
                    break;
            }
        }
        public override void Delete()
        {

        }

        public override void Update()
        {

        }
        /// <summary>
        /// 생성 함수 기본 default 타일 형태를 set 하고 리턴함 데이터를 알아서 넣어 셋팅해야 한다
        /// </summary>
        public Transform Pooling(Define.BaseType _type, string _objName, Transform _parent = null)
        {
            Transform ret = null;
            Transform parentTrf = parentTrfDic[_type].transform;
            // start 후에 이 부분만 partial하여 resmanager를 만들어서 관리
            string path = "";

            switch (_type)
            {
                case Define.BaseType.TILE:
                    path = string.Format("{0}/{1}", StaticDefine.TILE_PATH, _objName);
                    break;
                case Define.BaseType.CHARACTER:
                    path = string.Format("{0}/{1}", StaticDefine.TILE_PATH, _objName);
                    break;
                case Define.BaseType.BUILD:
                    path = string.Format("{0}/{1}", StaticDefine.TILE_PATH, _objName);
                    break;
                case Define.BaseType.ITEM:
                    path = string.Format("{0}/{1}", StaticDefine.TILE_PATH, _objName);
                    break;
            }
            // end

            if (parentTrf.childCount == 0)
            {
                // active ture or false 는 portrait 때문에 한건데 일단 보류
                ret = GameUtil.InstantiateResource<Transform>(path);
                ret.parent = parentTrf;
                ret.gameObject.SetActive(false);
            }
            else
            {
                ret = parentTrf.GetChild(0);
            }

            ret.gameObject.SetActive(true);
            ret.parent = _parent;

            return ret;
        }
        /// <summary>
        /// 회수 함수 
        /// </summary>
        public void Retrieve(Define.BaseType _type, Transform _trf)
        {
            _trf.parent = parentTrfDic[_type];
            _trf.localPosition = Vector3.zero;
            _trf.localRotation = Quaternion.identity;
            _trf.localScale = Vector3.one;
        }
    }
}
