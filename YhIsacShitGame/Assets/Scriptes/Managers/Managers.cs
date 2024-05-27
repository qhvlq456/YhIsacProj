using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YhProj.Game.Map;
using YhProj.Game.UI;
using YhProj.Game.GameInput;
using YhProj.Game.Player;
using Newtonsoft.Json;
using System.IO;

using System.Text;
using System.Linq;
using YhProj.Game.Play;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YhProj.Game
{
    public class Managers : Singleton<Managers>
    {
        public event Action<Transform> OnLookTargetChanged;
#if UNITY_EDITOR
        private  ExecutionData executionData;
#endif
        // 후에 lazy로 매니저 클래스를 변경할지 생각해 봐야 함
        private List<BaseManager> baseManagerList = new List<BaseManager>();

        public List<IGameFlow> GameFlowManagerList => baseManagerList.OfType<IGameFlow>().ToList();

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
#endif
            // manager set and define
            // load에서 순서와 상관없이 동작해야 함 // 즉 data만 셋팅? 그럼 ijson이 필요할지도?
            // lazy로 하여 필요할 때만 호출 하겠끔 해야겠다 get함수를 만들어 사용할 것!!

            lookTarget = GameUtil.AttachObj<Transform>("LookTarget");
            // 카메라도 게임모드에 따라 변경 시도 할 것ㅇ미
            testCamera = GameUtil.AttachObj<TestCamera>("Main Camera");
            testCamera.target = lookTarget;
            lookTarget.transform.position = StaticDefine.START_POSITION;

            RegisterManager(new PlayerManager(new PlayerInfo()));
            RegisterManager(new ObjectPoolManager());
            RegisterManager(new InputManager(lookTarget));
            RegisterManager(new MapManager()); // 나중에 순서에 상관없이 load되게 끔 변경해야 됨
            RegisterManager(new UIManager());
        }
        private void Start()
        {
            
            LoadAllManagers();
            // EventMediator.Instance.LoadSequnceEvent(GetManager<PlayerManager>().playerInfo);
        }
        private void Update()
        {
            foreach (var manager in baseManagerList)
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
            foreach (var manager in baseManagerList)
            {
                manager.Load();
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
        public void DisposeAllManagers()
        {
            foreach (var manager in baseManagerList)
            {
                manager.Dispose();
            }
        }
        public T GetManager<T>() where T : BaseManager, new()
        {
            T ret = null;

            for (int i = 0; i < baseManagerList.Count; i++)
            {
                if (baseManagerList[i].GetType() == typeof(T))
                {
                    ret = baseManagerList[i] as T;
                }
            }

            if (ret == null)
            {
                ret = new T();
                RegisterManager(ret);
            }

            return ret;
        }
    }

    public static class GameUtil
    {
        #region Json Util
        static T JsonToData<T>(string _json)
        {
            return JsonConvert.DeserializeObject<T>(_json);
        }
        static string DataToJson(object _obj)
        {
            return JsonConvert.SerializeObject(_obj);
        }
        public static void CreateJsonFile(string _createPath, string _fileName, object _jsonData)
        {
            string jsonData = DataToJson(_jsonData);
            string filePath = string.Format("{0}/{1}/{2}", Application.dataPath, _createPath, _fileName);

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    byte[] data = Encoding.UTF8.GetBytes(jsonData);
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Close();
                }

                Debug.Log($"JSON file created successfully at: {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create JSON file. Exception: {e.Message}");
            }
        }
        public static T LoadJson<T>(string _loadPath, string _fileName)
        {
            string filePath = string.Format("{0}/{1}/{2}", Application.dataPath, _loadPath, _fileName);

            if (File.Exists(filePath))
            {
                FileStream fileStream = new FileStream(Path.Combine(Application.dataPath + _loadPath + _fileName), FileMode.Open);
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
                fileStream.Close();
                string jsonData = Encoding.UTF8.GetString(data);
                return JsonToData<T>(jsonData);
            }
            else
            {
                Debug.LogWarningFormat("Utile LoadJson Warning \n filePath : {0}, _loadPath : {1}, _fileName : {2}}", filePath, _loadPath, _fileName);
                return default;
            }
        }

        public static List<T> LoadJsonArray<T>(string _loadPath, string _fileName)
        {
            string filePath = string.Format("{0}/{1}/{2}", Application.dataPath, _loadPath, _fileName);

            if (File.Exists(filePath))
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
                fileStream.Close();
                string jsonData = Encoding.UTF8.GetString(data);
                return JsonToData<List<T>>(jsonData);
            }
            else
            {
                Debug.LogWarningFormat("Util LoadJsonArray Warning \n {0}", filePath);
                return new List<T>(); // 또는 예외 처리를 추가하여 반환값을 선택할 수 있습니다.
            }
        }
        #endregion

        #region Resource Util
        public static T InstantiateResource<T>(string _path) where T : UnityEngine.Object
        {
            GameObject resObj = Resources.Load<GameObject>(_path);

            if (resObj == null)
            {
                Debug.LogWarningFormat("Util GetResource resobj Warning \n resobj : {0}, path : {1}", resObj, _path);
                return null;
            }

            T copyObj = UnityEngine.Object.Instantiate(resObj).GetComponent<T>();

            return copyObj;
        }
        #endregion

        #region Load BundleData
        public static void LoadBunlde()
        {
            // 정의 필요
        }
        #endregion

        #region Attach Obj
        // 후에 find 이름을 찾는 법도 만들어야 할 듯?
        public static T AttachObj<T>(string _name = null) where T : Component
        {
            T ret = null;

            string name = _name;

            if (string.IsNullOrEmpty(name))
            {
                name = typeof(T).Name;
            }

            GameObject _target = GameObject.Find(name);

            if (_target == null)
            {
                GameObject container = new GameObject(name);
                _target = container;
            }

            ret = _target.GetComponent<T>();

            if (ret == null)
            {
                ret = _target.AddComponent<T>();
            }

            return ret;
        }

        public static T AttachObj<T>(GameObject _go, string _name = null) where T : Component
        {
            string name = _name;

            if (string.IsNullOrEmpty(name))
            {
                name = typeof(T).Name;
            }

            _go = GameObject.Find(name);

            if (_go == null)
            {
                GameObject container = new GameObject(name);
                _go = container;
            }

            T component = _go.GetComponent<T>();

            if (component == null)
            {
                component = _go.AddComponent<T>();
            }

            return component;
        }

        #endregion Attach Obj
    }
}
