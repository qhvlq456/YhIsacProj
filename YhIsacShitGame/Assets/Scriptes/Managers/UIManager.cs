using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace YhProj.Game.UI
{
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
    public class UIManager : BaseManager
    {
        [SerializeField]
        string uiRootName = "UIRoot";

        List<UIInfo> uiInfoList = new List<UIInfo>();

        Transform root;

        // main ui만 따로 
        MainUI mainUI;
        Dictionary<Define.UIRootType, Transform> rootTrfDic = new Dictionary<Define.UIRootType, Transform>();
        public bool IsActiveUI
        {
            get
            {
                bool ret = false;

                foreach (var ui in rootTrfDic)
                {
                    if (ui.Key == Define.UIRootType.MAIN_UI)
                    {
                        continue;
                    }

                    var parent = ui.Value;

                    if (parent.childCount > 0 && parent.GetChild(parent.childCount - 1).gameObject.activeSelf)
                    {
                        ret = true;
                        break;
                    }
                }

                return ret;
            }
        }
        public override void Load(Define.GameMode _gameMode)
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
            uiInfoList.AddRange(uiData.mainUIDataList);
            uiInfoList.AddRange(uiData.popupUIDataList);
            uiInfoList.AddRange(uiData.tooltipUIDataList);
            uiInfoList.AddRange(uiData.contextualUIDataList);
            uiInfoList.AddRange(uiData.testUIDataList);

            // 1은 main ui이기 때문에 제외
            for (int i = 0; i < (int)Define.UIRootType.COUNT; i++)
            {
                string name = string.Format("{0}", (Define.UIRootType)i);
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

                if (!rootTrfDic.ContainsKey((Define.UIRootType)i))
                {
                    rootTrfDic.Add((Define.UIRootType)i, child.transform);
                }
            }

            string mainUIName = "";

            // 메인 UI 변경이 필요 그리고 loading 등등 조치가 필요하긴 함.. 씬전환등
            switch (Managers.Instance.gameMode)
            {
                case Define.GameMode.TEST:
                case Define.GameMode.EDITOR:
                    // 테스트 모드의 메인 UI를 셋팅함
                    foreach (var trf in rootTrfDic)
                    {
                        bool isActive = trf.Key == Define.UIRootType.TEST_UI;
                        trf.Value.gameObject.SetActive(isActive);
                    }
                    mainUIName = "MapToolMainUI";
                    break;
            }

            rootTrfDic[Define.UIRootType.MAIN_UI].gameObject.SetActive(true);
            mainUI = ShowUI<MapToolMainUI>(mainUIName);
        }

        public override void Update()
        {

        }
        public override void Delete()
        {

        }

        public BaseUI GetUI(UIInfo _uiInfo)
        {
            List<BaseUI> baseUIList = new List<BaseUI>();

            Transform root = rootTrfDic[_uiInfo.uiRootType];
            BaseUI baseUI = null;

            for (int i = 0; i < root.childCount; i++)
            {
                BaseUI ui = root.GetChild(i).GetComponent<BaseUI>();

                if (_uiInfo.name == ui.uiInfo.name)
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
                string path = "UI/MapTool/";

                baseUI = GameUtil.InstantiateResource<BaseUI>(path + _uiInfo.name);
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

        /// <summary>
        /// 부모의 자식 갯수를 통해 order를 정하여 UI를 표시하는 함수
        /// 스택과 같이 제일 하위에 생성됨
        /// </summary>
        /// <typeparam name="T"> ui 에 장착되어 있는 컴포넌트를 반환 </typeparam>
        /// <typeparam name="V">UI를 생성하고 셋팅을 위한 데이터의 타입</typeparam>
        /// <param name="_rootType"> 생성될 UI 오브젝트가 부모로 섬기게 될 타입 </param>
        /// <param name="_uiData"> UI를 생성하고 셋팅을 위한 데이터 </param>
        /// <returns> 패널의 컴포넌트 </returns>

        public T ShowUI<T, V>(string _uiName, V _param = null) where T : Component where V : BaseObject
        {
            UIInfo uiInfo = uiInfoList.Find(u => u.name == _uiName);

            if (uiInfo == null)
            {
                Debug.LogError("Not UIInfo Set Please Check your uidata.asset");
                return default(T);
            }

            BaseUI baseUI = GetUI(uiInfo);

            baseUI.Show(uiInfo, _param);

            return baseUI.GetComponent<T>();
        }
        public T ShowUI<T>(string _uiName) where T : Component
        {
            UIInfo uiInfo = uiInfoList.Find(u => u.name == _uiName);

            if (uiInfo == null)
            {
                Debug.LogError("Not UIInfo Set Please Check your uidata.asset");
                return default(T);
            }

            BaseUI baseUI = GetUI(uiInfo);

            baseUI.Show(uiInfo);

            return baseUI.GetComponent<T>();
        }

        public void HideUI(UIInfo _uiInfo)
        {
            List<BaseUI> baseUIList = new List<BaseUI>();

            Transform root = rootTrfDic[_uiInfo.uiRootType];
            BaseUI baseUI = null;

            int idx = root.childCount;

            // 순서에 맞게 뎁스도 조정하여야 함
            for (int i = 0; i < root.childCount; i++)
            {
                BaseUI ui = root.GetChild(i).GetComponent<BaseUI>();

                if (_uiInfo.name == ui.uiInfo.name)
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
        public void HideUI(string _name)
        {
            // name은 방식을 바꾸어야 할 듯?
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

        public static GameObject FindChild(GameObject _go, string name = null, bool _recursive = false)
        {
            Transform trf = FindChild<Transform>(_go, name, _recursive);

            if (trf == null)
            {
                return null;
            }

            return trf.gameObject;

        }
    }
}

