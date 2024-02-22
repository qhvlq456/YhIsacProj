namespace YhProj
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class BaseUI : MonoBehaviour
    {
        // ���� UI button, label����� ĳ����
        protected Dictionary<Type, List<UnityEngine.Object>> uiObjDic = new Dictionary<Type, List<UnityEngine.Object>>();

        public UIInfo uiInfo { get; private set; }

        // ugui�� canvas���� ������ ������ �׷��� Ȯ�� �Ǵ� �ʿ�� �� �гθ��� canvas�� ������ �Ǵ��Ͽ��� ��
        public int depth;

        // �ٵ� bind�ɾ �Ұ� ������
        protected void Bind<T>() where T : UnityEngine.Object
        {
            // �Ŀ� ����
        }
        public virtual void Show(UIInfo _uiInfo)
        {
            uiInfo = _uiInfo;
            gameObject.SetActive(true);
        }
        public virtual void Show<T>(UIInfo _uiInfo, T _param) where T : BaseObject
        {
            Show(_uiInfo);
        }
        public virtual void Hide()
        {
            // �Ŀ� ����
            gameObject.SetActive(false);
        }
    }
}
