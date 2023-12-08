using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YhProj;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Managers : Singleton<Managers>, YhProj.ILogger
{
#if UNITY_EDITOR
    ExecutionData executionData;
#endif
    public Define.GameMode gameMode { private set; get; }
    Define.DebugLogeer logType;

    List<BaseManager> baseManagerList = new List<BaseManager>();


    // maptool mode�� �� ������?�� ���� �� ���� �ʿ��ϱ� �ѵ�.. �ӽ÷� ���� ����ұ�?
    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        if (executionData == null)
        {
            executionData = AssetDatabase.LoadAssetAtPath<ExecutionData>(StaticDefine.EXECUTIONDATA_PATH);
        }

        gameMode = executionData.gameMode;
        logType = executionData.logType;
#endif
        // manager set and define
        // load���� ������ ������� �����ؾ� �� // �� data�� ����? �׷� ijson�� �ʿ�������?
        // lazy�� �Ͽ� �ʿ��� ���� ȣ�� �ϰڲ� �ؾ߰ڴ� get�Լ��� ����� ����� ��!!

        RegisterManager(new PlayerManager());
        RegisterManager(new MapManager());
        RegisterManager(new ObjectPoolManager());
        RegisterManager(new UIManager());
        RegisterManager(new LogManager());
    }
    private void Start()
    {
        LoadAllManagers();
    }
    public void RegisterManager(BaseManager _baseManager)
    {
        if (!baseManagerList.Contains(_baseManager))
        {
            baseManagerList.Add(_baseManager);
        }
    }
    public void LoadAllManagers()
    {
        foreach(var manager in baseManagerList)
        {
            manager.Load(gameMode);
        }
    }
    public void UpdateAllManagers()
    {
        foreach (var manager in baseManagerList)
        {
            manager.Update();
        }
    }
    public void DeleteAllManagers()
    {
        foreach (var manager in baseManagerList)
        {
            manager.Delete();
        }
    }
    public T GetManager<T>() where T : BaseManager, new()
    {
        T ret = null;

        for(int i = 0; i < baseManagerList.Count; i++)
        {
            if(baseManagerList[i].GetType() == typeof(T))
            {
                ret = baseManagerList[i] as T;
            }
        }

        if(ret == null)
        {
            ret = new T();
            RegisterManager(ret);
        }

        return ret;
    }

    public void Logger()
    {
        throw new NotImplementedException();
    }
}
