using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;


public class ObjectPoolManager : BaseManager
{
    // object pool�� �ֻ�� root �θ�
    Transform root;
    // object pool ���� �θ� �ڽĵ��� ������ ���� ���� ���ɰ� ȸ���� �����Ѵ�
    Dictionary<Define.BaseType,Transform> parentTrfDic = new Dictionary<Define.BaseType, Transform>();
    public override void Load()
    {

    }
    public override void Delete()
    {

    }

    public override void Update()
    {

    }
    /// <summary>
    /// ���� �Լ� �⺻ default Ÿ�� ���¸� set �ϰ� ������ �����͸� �˾Ƽ� �־� �����ؾ� �Ѵ�
    /// </summary>
    public Transform Pooling(Define.BaseType _type, string _objName ,Transform _parent = null)
    {
        Transform ret = null;
        Transform parentTrf = parentTrfDic[_type].transform;
        // start �Ŀ� �� �κи� partial�Ͽ� resmanager�� ���� ����
        string path = "";

        switch(_type)
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
            // active ture or false �� portrait ������ �Ѱǵ� �ϴ� ����
            parentTrf.gameObject.SetActive(true);
            GameObject copyObj = Util.InstantiateResource<GameObject>(path);
            ret = copyObj.transform;
            parentTrf.gameObject.SetActive(false);
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
    /// ȸ�� �Լ� 
    /// </summary>
    public void Retrieve(Define.BaseType _type, Transform _trf, int _childNum)
    {
        _trf.parent = parentTrfDic[_type].transform.GetChild(_childNum);
        _trf.localPosition = Vector3.zero;
        _trf.localRotation = Quaternion.identity;
        _trf.localScale = Vector3.one;
    }
}
