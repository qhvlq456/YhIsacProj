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
        // load에서 순서와 상관없이 동작해야 함 // 즉 data만 셋팅? 그럼 ijson이 필요할지도?
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
            manager.Load();
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
    public T GetManager<T>() where T : BaseManager
    {
        T ret = null;

        for(int i = 0; i < baseManagerList.Count; i++)
        {
            if(baseManagerList[i].GetType() == typeof(T))
            {
                ret = baseManagerList[i] as T;
            }
        }

        return ret;
    }

    public void Logger()
    {
        throw new NotImplementedException();
    }
}
