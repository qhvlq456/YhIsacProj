using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using YhProj;


public class ObjectPoolManager : BaseManager
{
    // object pool�� �ֻ�� root �θ�
    Transform root;
    // object pool ���� �θ� �ڽĵ��� ������ ���� ���� ���ɰ� ȸ���� �����Ѵ�
    Dictionary<Define.BaseType,Transform> parentTrfDic = new Dictionary<Define.BaseType, Transform>();

    public override void Load(Define.GameMode _gameMode)
    {
        root = Util.AttachObj<Transform>("ObjectPoolRoot");

        for (int i = 0; i < (int)Define.BaseType.COUNT; i++)
        {
            Define.BaseType baseType = (Define.BaseType)i;
            string objName = baseType.ToString();
            GameObject go = new GameObject(objName);
            go.transform.parent = root;
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
            ret = Util.InstantiateResource<Transform>(path);
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
    /// ȸ�� �Լ� 
    /// </summary>
    public void Retrieve(Define.BaseType _type, Transform _trf)
    {
        _trf.parent = parentTrfDic[_type];
        _trf.localPosition = Vector3.zero;
        _trf.localRotation = Quaternion.identity;
        _trf.localScale = Vector3.one;
    }
}
