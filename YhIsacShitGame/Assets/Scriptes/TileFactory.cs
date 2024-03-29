using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace YhProj.Game.Map
{
    public class TileFactory : IFactory
    {
        public T Create<T>(BaseData _value) where T : BaseObject 
        {
            // res이름도 넣어야 할 듯?
            Transform trf = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(_value.type, _value.resName);

            if (trf == null)
            {
                // 오브젝트 풀이 비어있을 경우의 처리
                return null;
            }

            T ret = trf.GetComponent<T>();

            if (ret == null)
            {
                ret = trf.gameObject.AddComponent<T>();
            }

            // Load 메서드의 구현에 따라서 실제 로딩 방식이 달라질 수 있음
            ret.Create(_value);

            return ret;
        }

        public T Create<T>(BaseData _data, Vector3 _position) where T : BaseObject
        {
            T ret = Create<T>(_data);
            ret.transform.position = _position;
            return ret;
        }

        public T Create<T>(BaseData _data, Transform _parent) where T : BaseObject
        {
            T ret = Create<T>(_data);
            ret.transform.SetParent(_parent);
            return ret;
        }

        public T Create<T>(BaseData _data, Transform _parent, Vector3 _position) where T : BaseObject
        {
            T ret = Create<T>(_data);
            ret.transform.SetParent(_parent);
            ret.transform.localPosition = _position;
            return ret;
        }
    }
}


