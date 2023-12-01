using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using YhProj;

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

    Transform root;
    Dictionary<Define.UIRootType, Transform> rootTrfDic = new Dictionary<Define.UIRootType, Transform>();

    public override void Load()
    {
        if(root == null)
        {
            root = GameObject.Find(uiRootName).transform;

            if(root == null)
            {
                GameObject container = new GameObject(uiRootName);
                Canvas canvas = container.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                // 해상도에 따라 크기 조정
                CanvasScaler canvasScaler = container.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(1920, 1080); // 후에 리솔루션 고정 값 넣을 것임 screen 함수라든지?

                // 이벤트 전파를 위한 설정
                container.AddComponent<GraphicRaycaster>();

                root = container.transform;
            }
        }

        for(int i = 0; i < (int)Define.UIRootType.COUNT; i++)
        {
            string name = string.Format("{0}", (Define.UIRootType)i);
            GameObject child = new GameObject(name);

            child.transform.parent = root;
            child.transform.localPosition = Vector3.zero;

            if(!rootTrfDic.ContainsKey((Define.UIRootType)i))
            {
                rootTrfDic.Add((Define.UIRootType)i, child.transform);
            }
        }

        // 메인 UI 변경이 필요 그리고 loading 등등 조치가 필요하긴 함.. 씬전환등
        switch(Managers.Instance.gameMode)
        {
            case Define.GameMode.ANDROID:
                break;
            case Define.GameMode.IOS:
                break;
            case Define.GameMode.EDITOR:
                break;
            case Define.GameMode.TEST:
                // 테스트 모드의 메인 UI를 셋팅함
                foreach(var trf in rootTrfDic)
                {
                    bool isActive = trf.Key == Define.UIRootType.TEST_UI;
                    trf.Value.gameObject.SetActive(isActive);
                }
                break;
        }
    }

    public override void Update()
    {
        
    }
    public override void Delete()
    {
        
    }
    /// <summary>
    /// 부모의 자식 갯수를 통해 order를 정하여 UI를 표시하는 함수
    /// 스택과 같이 제일 하위에 생성됨
    /// </summary>
    /// <typeparam name="T"> ui 에 장착되어 있는 컴포넌트를 반환 </typeparam>
    /// <param name="_rootType"> 생성될 UI 오브젝트가 부모로 섬기게 될 타입 </param>
    /// <param name="_uiData"> UI를 생성하고 셋팅을 위한 데이터 </param>
    /// <returns></returns>

    public T ShowUI<T>(UIData _uiData) where T : Component
    {
        List<BaseUI> baseUIList = new List<BaseUI>();

        Transform root = rootTrfDic[_uiData._uiRootType];
        BaseUI baseUI = null;

        for (int i = 0; i < root.childCount; i++)
        {
            BaseUI ui = root.GetChild(i).GetComponent<BaseUI>();

            if(ui.uiData.name == _uiData.name)
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
            baseUI = Util.InstantiateResource<BaseUI>(_uiData.name);
            baseUI.transform.parent = root;
            baseUI.transform.SetSiblingIndex(root.childCount - 1);
        }

        int depth = 0;

        // depth 새로 조정
        for(int i = 0; i < baseUIList.Count; i++)
        {
            if(baseUIList[i].gameObject.activeSelf)
            {
                baseUIList[i].depth = depth;
                depth++;
            }
        }

        baseUI.Show(_uiData);

        return baseUI.GetComponent<T>();
    }
    public void HideUI(BaseUI _baseUI)
    {
        List<BaseUI> baseUIList = new List<BaseUI>();

        Transform root = rootTrfDic[_baseUI.uiData._uiRootType];
        BaseUI baseUI = null;

        int idx = root.childCount;

        // 순서에 맞게 뎁스도 조정하여야 함
        for (int i = 0; i < root.childCount; i++)
        {
            BaseUI ui = root.GetChild(i).GetComponent<BaseUI>();

            if (_baseUI.uiData.name == ui.name)
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

        if(baseUI != null)
        {
            baseUI.Hide();
        }
    }
    public void HideUI(string _name)
    {
        // name은 방식을 바꾸어야 할 듯?
    }
}
