using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YhProj
{
    public class BaseUI : MonoBehaviour
    {
        protected Dictionary<Type, List<UnityEngine.Object>> uiObjDic = new Dictionary<Type, List<UnityEngine.Object>>();

        public UIData uiData;
        public int depth { set; get; }
        protected void Bind<T>() where T : UnityEngine.Object
        {
            // 후에 구현
        }
        public virtual void Show(UIData _uiData)
        {
            uiData = _uiData;
        }
        public virtual void Hide()
        {
            // 후에 구현
        }
    }
}
