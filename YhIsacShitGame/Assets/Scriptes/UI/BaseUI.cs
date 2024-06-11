using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YhProj.Game.UI
{
    public class BaseUI : MonoBehaviour
    {
        // 각종 UI button, label등등을 캐싱함
        protected Dictionary<Type, List<UnityEngine.Object>> uiObjDic = new Dictionary<Type, List<UnityEngine.Object>>();

        public UIInfo uiInfo;

        // ugui는 canvas에서 솔팅을 조절함 그래서 확인 또는 필요시 각 패널마다 canvas를 넣을지 판단하여야 함
        public int depth;

        // 근데 bind걸어서 할게 없음ㅋ
        protected void Bind<T>() where T : UnityEngine.Object
        {
            // 후에 구현
        }
        public virtual void Show(UIInfo _uiInfo)
        {
            uiInfo = _uiInfo;
            gameObject.SetActive(true);
        }
        // 후에 파라미터 제너릭이 아닌 클래스 args로 변경
        public virtual void Show<T>(UIInfo _uiInfo, T _param)
        {
            Show(_uiInfo);
        }
        public virtual void Hide()
        {
            // 후에 구현
            gameObject.SetActive(false);
        }
    }
}
