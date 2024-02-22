using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YhProj;
using static UnityEngine.GraphicsBuffer;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Managers : Singleton<Managers>, YhProj.ILogger
{
    public event Action<Transform> OnLookTargetChanged;

#if UNITY_EDITOR
    ExecutionData executionData;
#endif
    public Define.GameMode gameMode { private set; get; }
    Define.DebugLogeer logType;

    List<BaseManager> baseManagerList = new List<BaseManager>();


    // maptool mode일 때 데이터?를 저장 할 곳이 필요하긴 한데.. 임시로 여길 사용할까?
    // 일단 대기
    public Transform lookTarget;
    // 나중에 상속 구조 만들어서 사용
    // baseCamera -> fieldcamera/ uicamera / testcamera
    public TestCamera testCamera;

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        if (executionData == null)
        {
            executionData = AssetDatabase.LoadAssetAtPath<ExecutionData>(StaticDefine.SCRIPTABLEOBJECT_PATH + "ExecutionData.asset");
        }

        gameMode = executionData.gameMode;
        logType = executionData.logType;
#endif
        // manager set and define
        // load에서 순서와 상관없이 동작해야 함 // 즉 data만 셋팅? 그럼 ijson이 필요할지도?
        // lazy로 하여 필요할 때만 호출 하겠끔 해야겠다 get함수를 만들어 사용할 것!!

        lookTarget = Util.AttachObj<Transform>("LookTarget");
        // 카메라도 게임모드에 따라 변경 시도 할 것ㅇ미
        testCamera = Util.AttachObj<TestCamera>("Main Camera");
        testCamera.target = lookTarget;
        lookTarget.transform.position = StaticDefine.START_POSITION;

        RegisterManager(new PlayerManager(new PlayerInfo()));
        RegisterManager(new ObjectPoolManager());
        RegisterManager(new InputManager(lookTarget));
        RegisterManager(new MapManager()); // 나중에 순서에 상관없이 load되게 끔 변경해야 됨
        RegisterManager(new UIManager());
        RegisterManager(new LogManager());
    }
    private void Start()
    {
        LoadAllManagers();
        // EventMediator.Instance.LoadSequnceEvent(GetManager<PlayerManager>().playerInfo);
    }
    private void Update()
    {
        foreach(var manager in baseManagerList)
        {
            manager.Update();
        }
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
        // 나중에 업데이트만 가능한 매니처를 추려서 성능을 향상 시키자
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
