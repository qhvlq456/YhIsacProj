using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using YhProj;
using UnityEngine.UIElements;
using UnityEditor.PackageManager;
using UnityEngine.Rendering;

/*
 * main ui
 * popup ui
 * toolip ui
 * hud
 * notification
 * dialog
 * Contextual UI ?? 
 * �ʹ� ���� ĵ���� ����� ������ ���� ���ϸ� ���� �� �� �������� ��Ʈ ĵ���� ������ �� ������Ʈ�� �̿��� ī�װ� �з�
 */
public class UIManager : BaseManager
{
    [SerializeField]
    string uiRootName = "UIRoot";

    List<UIInfo> uiInfoList = new List<UIInfo>();

    Transform root;

    // main ui�� ���� 
    MainUI mainUI;
    Dictionary<Define.UIRootType, Transform> rootTrfDic = new Dictionary<Define.UIRootType, Transform>();
    public bool IsActiveUI
    {
        get
        {
            bool ret = false;

            foreach (var ui in rootTrfDic)
            {
                if(ui.Key == Define.UIRootType.MAIN_UI)
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
        root = Util.AttachObj<Transform>(uiRootName);

        Canvas canvas = root.gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // �ػ󵵿� ���� ũ�� ����
        CanvasScaler canvasScaler = root.gameObject.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height); // �Ŀ� ���ַ�� ���� �� ���� ���� screen �Լ������?

        // �̺�Ʈ ���ĸ� ���� ����
        root.gameObject.GetComponent<GraphicRaycaster>();

        // �Ŀ� ��ο� ���� �������� �ʿ���
        // ������ �ʿ��� �� ������..
        // uidata set 
        UIData uiData = Resources.Load<UIData>("ScriptableObjects/UIData");
        uiInfoList.AddRange(uiData.mainUIDataList);
        uiInfoList.AddRange(uiData.popupUIDataList);
        uiInfoList.AddRange(uiData.tooltipUIDataList);
        uiInfoList.AddRange(uiData.contextualUIDataList);
        uiInfoList.AddRange(uiData.testUIDataList);

        // 1�� main ui�̱� ������ ����
        for (int i = 0; i < (int)Define.UIRootType.COUNT; i++)
        {
            string name = string.Format("{0}", (Define.UIRootType)i);
            GameObject child = new GameObject(name);

            RectTransform childRect = child.AddComponent<RectTransform>();
            // strech
            childRect.anchorMin = Vector2.zero;
            childRect.anchorMax = Vector2.one;

            //Left, Bottom ����
            childRect.offsetMin = Vector2.zero;

            //Right, Top ���� -> ���ϴ� ũ�� - �����ػ󵵰�
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

        // ���� UI ������ �ʿ� �׸��� loading ��� ��ġ�� �ʿ��ϱ� ��.. ����ȯ��
        switch(Managers.Instance.gameMode)
        {
            case Define.GameMode.EDITOR:
                break;
            case Define.GameMode.TEST:
            case Define.GameMode.MAPTOOL:
                // �׽�Ʈ ����� ���� UI�� ������
                foreach(var trf in rootTrfDic)
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

        // ���� ���������� �ű�
        if (baseUI != null)
        {
            baseUI.transform.SetAsLastSibling();
        }
        else
        {
            string path = "UI/MapTool/";

            baseUI = Util.InstantiateResource<BaseUI>(path + _uiInfo.name);
            baseUI.transform.SetParent(root, false);
            baseUI.transform.localPosition = Vector3.zero;
            baseUI.transform.SetSiblingIndex(root.childCount - 1);
        }

        int depth = 0;

        // depth ���� ����
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
    /// �θ��� �ڽ� ������ ���� order�� ���Ͽ� UI�� ǥ���ϴ� �Լ�
    /// ���ð� ���� ���� ������ ������
    /// </summary>
    /// <typeparam name="T"> ui �� �����Ǿ� �ִ� ������Ʈ�� ��ȯ </typeparam>
    /// <typeparam name="V">UI�� �����ϰ� ������ ���� �������� Ÿ��</typeparam>
    /// <param name="_rootType"> ������ UI ������Ʈ�� �θ�� ����� �� Ÿ�� </param>
    /// <param name="_uiData"> UI�� �����ϰ� ������ ���� ������ </param>
    /// <returns> �г��� ������Ʈ </returns>

    public T ShowUI<T, V>(string _uiName, V _param = null) where T : Component where V : BaseObject
    {
        UIInfo uiInfo = uiInfoList.Find(u => u.name == _uiName);

        if(uiInfo == null)
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

        // ������ �°� ������ �����Ͽ��� ��
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

        if(baseUI != null)
        {
            baseUI.Hide();
        }
    }
    public void HideUI(string _name)
    {
        // name�� ����� �ٲپ�� �� ��?
    }
}
