using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Character
{
    public class CharacterFactory : IFactory
    {
        public V Create<T, V>(T _value) where T : BaseData where V : BaseObject
        {
            // Enum 타입인 경우 Enum의 이름을 문자열로 가져오기
            string typeName = Enum.GetName(typeof(Define.BaseType), _value.type);

            Transform trf = Managers.Instance.GetManager<ObjectPoolManager>().Pooling(_value.type, typeName);

            if (trf == null)
            {
                // 오브젝트 풀이 비어있을 경우의 처리
                return null;
            }

            V ret = trf.GetComponent<V>();

            if (ret == null)
            {
                ret = trf.gameObject.AddComponent<V>();
            }

            // Load 메서드의 구현에 따라서 실제 로딩 방식이 달라질 수 있음
            ret.Load(_value);

            return ret;
        }
    }
}

