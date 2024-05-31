using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;

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

        public UIInfo GetUIInfo(string _uiName)
        {
            UIInfo ret = uiInfoList.Find(x => x.uiName == _uiName);

            return ret;
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
        public void HideUI(string _uiName)
        {
            UIInfo info = uiInfoList.Find(x => x.uiName == _uiName);

            if(info != null)
            {
                HideUI(info.uiName);
            }
            else
            {
                Debug.LogError("[HideUI] Not found ui info");
            }
        }
        public void HideUI(UIInfo _uiInfo)
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

        public void HideAllUI()
        {
            for (int i = 0; i < uiInfoList.Count; i++)
            {
                HideUI(uiInfoList[i]);
            }
        }
    }
    public class UIManager : BaseManager
    {
        [SerializeField]
        private string uiRootName = "UIRoot";
        private Dictionary<UIRootType, UIDerived> rootDerivedMap = new Dictionary<UIRootType, UIDerived>();
        private readonly Dictionary<UIRootType, string> uiPathMap = new Dictionary<UIRootType, string>()
        {
            { UIRootType.Contextual, "UI/Contextual/" },
            { UIRootType.Popup, "UI/Popup/" },
            { UIRootType.Tooltip, "UI/Tooltip/" },
        };

        private List<MainUI> mainUIList = new List<MainUI>();

        public bool IsActiveUI
        {
            get
            {
                foreach (var ui in rootDerivedMap.Values)
                {
                    if (ui.IsActiveUI)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override void Load()
        {
            root = GameUtil.AttachObj<Transform>(uiRootName);

            InitializeCanvas(root);
            LoadUIData();
            CreateUIRoots();
        }

        private void InitializeCanvas(Transform root)
        {
            var canvas = root.gameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var canvasScaler = root.gameObject.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);

            root.gameObject.AddComponent<GraphicRaycaster>();
        }

        private void LoadUIData()
        {
            var uiData = Resources.Load<UIData>("ScriptableObjects/UIData");

            for (int i = 0; i < (int)UIRootType.Count; i++)
            {
                var rootType = (UIRootType)i;
                var uiDataList = GetUIDataListForRootType(uiData, rootType);
                var child = CreateUIRootObject(rootType);

                if (!rootDerivedMap.ContainsKey(rootType))
                {
                    var uIDerived = new UIDerived(uiPathMap[rootType], child.transform, uiDataList);
                    rootDerivedMap.Add(rootType, uIDerived);
                }
            }
        }

        private List<UIInfo> GetUIDataListForRootType(UIData uiData, UIRootType rootType)
        {
            return rootType switch
            {
                UIRootType.Tooltip => uiData.tooltipUIDataList,
                UIRootType.Popup => uiData.popupUIDataList,
                UIRootType.Contextual => uiData.contextualUIDataList,
                _ => new List<UIInfo>()
            };
        }

        private GameObject CreateUIRootObject(UIRootType rootType)
        {
            var name = $"{rootType}";
            var child = new GameObject(name);
            var childRect = child.AddComponent<RectTransform>();

            childRect.anchorMin = Vector2.zero;
            childRect.anchorMax = Vector2.one;
            childRect.offsetMin = Vector2.zero;
            childRect.offsetMax = Vector2.zero;

            child.transform.SetParent(root, false);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;

            return child;
        }
        private void CreateUIRoots()
        {
            for (int i = 0; i < (int)UIRootType.Count; i++)
            {
                var rootType = (UIRootType)i;
                if (!rootDerivedMap.ContainsKey(rootType))
                {
                    var child = CreateUIRootObject(rootType);
                    var uiDataList = new List<UIInfo>(); // Assuming you have a method to get UI data list for each root type
                    var uIDerived = new UIDerived(uiPathMap[rootType], child.transform, uiDataList);
                    rootDerivedMap.Add(rootType, uIDerived);
                }
            }
        }
        public override void Update()
        {
        }

        public override void Dispose()
        {
        }

        public T ShowUI<T>(UIRootType rootType, string uiName) where T : Component
        {
            var uiInfo = GetUIInfo(rootType, uiName);
            return ShowInternalUI<T>(uiInfo);
        }

        public T ShowUI<T>(string uiName) where T : Component
        {
            var uiInfo = GetUIInfo(uiName);
            return ShowInternalUI<T>(uiInfo);
        }

        public T ShowUI<T>(UIInfo uiInfo) where T : Component
        {
            return ShowInternalUI<T>(uiInfo);
        }

        private T ShowInternalUI<T>(UIInfo uiInfo) where T : Component
        {
            if (uiInfo == null)
            {
                Debug.LogError("UIInfo not found.");
                return null;
            }

            if (!rootDerivedMap.TryGetValue(uiInfo.uiRootType, out var derived))
            {
                Debug.LogError($"UIRootType: {uiInfo.uiRootType} not found.");
                return null;
            }

            var baseUI = derived.GetUI(uiInfo);
            baseUI.Show(uiInfo);

            return baseUI.GetComponent<T>();
        }

        public void HideUI(string _uiName)
        {
            UIDerived derived = rootDerivedMap.Values.First(x => x.IsConstains(_uiName));

            if(derived != null)
            {
                derived.HideUI(_uiName);
            }
            else
            {
                Debug.LogError($"_uiName : {_uiName}, not found.");
            }

        }

        public void HideUI(UIInfo uiInfo)
        {
            if (uiInfo == null)
            {
                Debug.LogError("UIInfo not found.");
                return;
            }

            if (rootDerivedMap.TryGetValue(uiInfo.uiRootType, out var derived))
            {
                derived.HideUI(uiInfo);
            }
            else
            {
                Debug.LogError($"UIRootType: {uiInfo.uiRootType} not found.");
            }
        }

        public void HideAllUI()
        {
            foreach (var derived in rootDerivedMap.Values)
            {
                derived.HideAllUI();
            }
        }

        private UIInfo GetUIInfo(UIRootType rootType, string uiName)
        {
            if (rootDerivedMap.TryGetValue(rootType, out var derived))
            {
                return derived.GetUIInfo(uiName);
            }
            Debug.LogError($"UIRootType: {rootType} not found.");
            return null;
        }

        private UIInfo GetUIInfo(string uiName)
        {
            foreach (var derived in rootDerivedMap.Values)
            {
                var uiInfo = derived.GetUIInfo(uiName);
                if (uiInfo != null)
                {
                    return uiInfo;
                }
            }
            Debug.LogError($"UIInfo not found for UIName: {uiName}");
            return null;
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

