using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game;

public class EnemyFactory : IFactory
{
    public T Create<T>(BaseData _value) where T : BaseObject
    {
        throw new System.NotImplementedException();
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
