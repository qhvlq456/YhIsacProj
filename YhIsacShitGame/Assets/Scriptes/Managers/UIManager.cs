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
 * �ʹ� ���� ĵ���� ����� ������ ���� ���ϸ� ���� �� �� �������� ��Ʈ ĵ���� ������ �� ������Ʈ�� �̿��� ī�װ� �з�
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

                // �ػ󵵿� ���� ũ�� ����
                CanvasScaler canvasScaler = container.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(1920, 1080); // �Ŀ� ���ַ�� ���� �� ���� ���� screen �Լ������?

                // �̺�Ʈ ���ĸ� ���� ����
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

        // ���� UI ������ �ʿ� �׸��� loading ��� ��ġ�� �ʿ��ϱ� ��.. ����ȯ��
        switch(Managers.Instance.gameMode)
        {
            case Define.GameMode.ANDROID:
                break;
            case Define.GameMode.IOS:
                break;
            case Define.GameMode.EDITOR:
                break;
            case Define.GameMode.TEST:
                // �׽�Ʈ ����� ���� UI�� ������
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
    /// �θ��� �ڽ� ������ ���� order�� ���Ͽ� UI�� ǥ���ϴ� �Լ�
    /// ���ð� ���� ���� ������ ������
    /// </summary>
    /// <typeparam name="T"> ui �� �����Ǿ� �ִ� ������Ʈ�� ��ȯ </typeparam>
    /// <param name="_rootType"> ������ UI ������Ʈ�� �θ�� ����� �� Ÿ�� </param>
    /// <param name="_uiData"> UI�� �����ϰ� ������ ���� ������ </param>
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

        // ���� ���������� �ű�
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

        // depth ���� ����
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

        // ������ �°� ������ �����Ͽ��� ��
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
        // name�� ����� �ٲپ�� �� ��?
    }
}
