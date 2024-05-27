using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace YhProj.Game.UI
{
    // uiroot 하위 랜더링할 캔버스 종류들
    public enum UIRootType
    {
        //Main, // 항상 고정값이 되어야 함
        Popup,
        Tooltip,
        Contextual,
        Count
    }
    /// <summary>
    /// ui가 닫히고 다른 ui를 띄우는경우
    /// ui가 모두 닫혀야 하는 경우도 생각하여야 함
    /// </summary>

    /*
     * main ui
     * popup ui
     * toolip ui
     * hud
     * notification
     * dialog
     * Contextual UI ?? 
     * 너무 많은 캔버스 사용은 렌더링 성능 저하를 가져 올 수 있음으로 루트 캔버스 하위에 빈 오브젝트를 이용해 카테고리 분류
     */

    // 만약 아래 클래스에서 다양성에 문제가 생긴다면 파생시켜서 관리할 것
    public class UIDerived
    {
        private readonly string path;

        protected Transform root;

        [SerializeField]
        private List<UIInfo> uiInfoList = new List<UIInfo>();
        public bool IsConstains(string _uiName) => uiInfoList.Find(x => x.uiName == _uiName) == null ? false : true;

        [SerializeField]
        protected List<BaseUI> displayUIList = new List<BaseUI>();
        public bool IsActiveUI
        {
            get => displayUIList.Count > 0;
        }

        public UIDerived(string _path, Transform _parent, List<UIInfo> _uiInfoList)
        {
            path = _path;
            root = _parent;
            uiInfoList = _uiInfoList;
        }
        public BaseUI GetUI(UIInfo _uiInfo)
        {
            List<BaseUI> baseUIList = new List<BaseUI>();
            BaseUI baseUI = null;

            for (int i = 0; i < root.childCount; i++)
            {
                BaseUI ui = root.GetChild(i).GetComponent<BaseUI>();

                if (_uiInfo.uiName == ui.uiInfo.uiName)
                {
                    baseUI = ui;
                }

                baseUIList.Add(ui);
            }

            // 제일 마지막으로 옮김
            if (baseUI != null)
            {
                baseUI.transform.SetAsLastSibling();
            }
            else
            {
                baseUI = GameUtil.InstantiateResource<BaseUI>(path + _uiInfo.uiName);
                baseUI.transform.SetParent(root, false);
                baseUI.transform.localPosition = Vector3.zero;
                baseUI.transform.SetSiblingIndex(root.childCount - 1);
            }

            int depth = 0;

            // depth 새로 조정
            for (int i = 0; i < baseUIList.Count; i++)
            {
                if (baseUIList[i].gameObject.activeSelf)
                {
                    baseUIList[i].depth = depth;
                    depth++;
                }
            }

            return baseUI;
        }

        public T ShowUI<T>(string _uiName) where T : Component
        {
            UIInfo uiInfo = uiInfoList.Find(u => u.uiName == _uiName);

            if (uiInfo == null)
            {
                Debug.LogError($"UIRootType : , Show UI Not UIInfo Set Please Check your uidata.asset");
                return default(T);
            }

            BaseUI baseUI = GetUI(uiInfo);

            baseUI.Show(uiInfo);

            return baseUI.GetComponent<T>();
        }
        public void Hide(UIInfo _uiInfo)
        {
            List<BaseUI> baseUIList = new List<BaseUI>();
            BaseUI baseUI = null;

            int idx = root.childCount;

            // 순서에 맞게 뎁스도 조정하여야 함
            for (int i = 0; i < root.childCount; i++)
            {
                BaseUI ui = root.GetChild(i).GetComponent<BaseUI>();

                if (_uiInfo.uiName == ui.uiInfo.uiName)
                {
                    baseUI = ui;
                }

                baseUIList.Add(ui);
            }

            int depth = 0;

            for (int i = 0; i < root.childCount; i++)
            {
                if (baseUIList[i].gameObject.activeSelf)
                {
                    baseUIList[i].depth = depth;
                    depth++;
                }
            }

            if (baseUI != null)
            {
                baseUI.Hide();
            }
        }

        public void AllHide()
        {
            for (int i = 0; i < uiInfoList.Count; i++)
            {
                Hide(uiInfoList[i]);
            }
        }
    }
    public class UIManager : BaseManager
    {
        //internal static readonly Lazy<UIManager> Lazy = new Lazy<UIManager>(() =>
        //{
        //    if (!NeonSdkService.IsInitialized)
        //        throw new NeonException("NeonSdk is not initialized.");
        //    return new NeonAuth();
        //});

        [SerializeField]
        private string uiRootName = "UIRoot";
        private Dictionary<UIRootType, UIDerived> rootDerivedMap = new Dictionary<UIRootType, UIDerived>();

        private readonly Dictionary<UIRootType, string> uIPathMap = new Dictionary<UIRootType, string>()
        {
            { UIRootType.Contextual, "UI/Contextual/"},
            { UIRootType.Popup, "UI/Popup/"},
            { UIRootType.Tooltip, "UI/Tooltip/"},
        };


        // main ui만 따로 
        private List<MainUI> mainUIList = new List<MainUI>();
        public bool IsActiveUI
        {
            get
            {
                bool ret = false;

                foreach (var ui in rootDerivedMap)
                {
                    var parent = ui.Value;

                    if (parent.IsActiveUI)
                    {
                        ret = true;
                        break;
                    }
                }

                return ret;
            }
        }
        public override void Load()
        {
            root = GameUtil.AttachObj<Transform>(uiRootName);

            Canvas canvas = root.gameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // 해상도에 따라 크기 조정
            CanvasScaler canvasScaler = root.gameObject.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height); // 후에 리솔루션 고정 값 넣을 것임 screen 함수라든지?

            // 이벤트 전파를 위한 설정
            root.gameObject.GetComponent<GraphicRaycaster>();

            // 후에 경로에 대한 재지정이 필요함
            // 통일이 필요할 것 같은데..
            // uidata set 
            UIData uiData = Resources.Load<UIData>("ScriptableObjects/UIData");



            // 1은 main ui이기 때문에 제외
            for (int i = 0; i < (int)UIRootType.Count; i++)
            {
                string name = string.Format("{0}", (UIRootType)i);
                GameObject child = new GameObject(name);

                RectTransform childRect = child.AddComponent<RectTransform>();
                // strech
                childRect.anchorMin = Vector2.zero;
                childRect.anchorMax = Vector2.one;

                //Left, Bottom 변경
                childRect.offsetMin = Vector2.zero;

                //Right, Top 변경 -> 원하는 크기 - 기준해상도값
                childRect.offsetMax = new Vector2(Screen.width, Screen.height);

                child.transform.SetParent(root, false);
                child.transform.localPosition = Vector3.zero;
                child.transform.localScale = Vector3.one;

                UIRootType rootType = (UIRootType)i;
                List<UIInfo> uiDataList = new List<UIInfo>();

                switch(rootType)
                {
                    case UIRootType.Tooltip:
                        uiDataList = uiData.tooltipUIDataList;
                        break;
                    case UIRootType.Popup:
                        uiDataList = uiData.popupUIDataList;
                        break;
                    case UIRootType.Contextual:
                        uiDataList = uiData.contextualUIDataList;
                        break;
                }

                // uiInfoList.AddRange(uiData.mainUIDataList);

                if (!rootDerivedMap.ContainsKey(rootType))
                {
                    UIDerived uIDerived = new UIDerived(uIPathMap[rootType], child.transform, uiDataList);

                    rootDerivedMap.Add((UIRootType)i, uIDerived);
                }
            }

            string mainUIName = "";

            // 메인 UI 변경이 필요 그리고 loading 등등 조치가 필요하긴 함.. 씬전환등

            // rootTrfDic[UIRootType.MAIN_UI].gameObject.SetActive(true);
            // mainUI = ShowUI<MapToolMainUI>(mainUIName);
        }

        public override void Update()
        {

        }
        public override void Dispose()
        {

        }

        public BaseUI GetUI(UIInfo _uiInfo)
        {
            return default(BaseUI);
        }

        /// <summary>
        /// 부모의 자식 갯수를 통해 order를 정하여 UI를 표시하는 함수
        /// 스택과 같이 제일 하위에 생성됨
        /// </summary>
        /// <typeparam name="T"> ui 에 장착되어 있는 컴포넌트를 반환 </typeparam>
        /// <typeparam name="V">UI를 생성하고 셋팅을 위한 데이터의 타입</typeparam>
        /// <param name="_rootType"> 생성될 UI 오브젝트가 부모로 섬기게 될 타입 </param>
        /// <param name="_uiData"> UI를 생성하고 셋팅을 위한 데이터 </param>
        /// <returns> 패널의 컴포넌트 </returns>

        public T ShowUI<T, V>(UIRootType _rootType, string _uiName, V _param = null) where T : Component where V : BaseObject
        {
            return default(T);
        }
        public T ShowUI<T>(UIRootType _rootType, string _uiName) where T : Component
        {


            return default(T);
        }
        public T ShowUI<T>(UIInfo _uiInfo) where T : Component
        {


            return default(T);
        }
        public void ShowUI(UIRootType _rootType, string _uiName)
        {
            UIDerived derived = null;



        }

        public void ShowUI(UIInfo _uiInfo)
        {

        }

        public void HideUI(UIInfo _uiInfo)
        {
            
        }

        public void HideUI(string _uiName)
        {
            
        }
        /// <summary>
        /// 코루틴으로 패널을 닫는 경우가 있음 그래서 닫고나서 열린다는 보장을 할 수 없다.
        /// 그래서 방법을 생각해야 한다
        /// </summary>
        public void AllHide()
        {
            
        }
    }




    public static class UIUtil
    {
        public static T FindChild<T>(GameObject _go, string _name = null, bool _isRecursive = false) where T : UnityEngine.Object
        {
            if (_go == null)
            {
                return null;
            }

            if (!_isRecursive)
            {
                for (int i = 0; i < _go.transform.childCount; i++)
                {
                    Transform child = _go.transform.GetChild(i);

                    if (string.IsNullOrEmpty(child.name) || child.name == _name)
                    {
                        T component = child.GetComponent<T>();

                        return component;
                    }
                }
            }
            else
            {
                foreach (T component in _go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(_name) || component.name == _name)
                    {
                        return component;
                    }
                }
            }

            return null;
        }

        public static GameObject FindChild(GameObject _go, string _name = null, bool _recursive = false)
        {
            Transform trf = FindChild<Transform>(_go, _name, _recursive);

            if (trf == null)
            {
                return null;
            }

            return trf.gameObject;

        }
    }
}

