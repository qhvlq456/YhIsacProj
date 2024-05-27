using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;

namespace YhProj.Game.UI
{
    public class MainUI : MonoBehaviour
    {
        /// notch대응을 위해 그룹별로 나누는 작업이 있음 좋을 것 같다
        public class UIParts
        {
            // area영역의 중앙
            public Transform root;
            // 해당 실제 ui영역
            private Rect rect;
            private List<BaseUI> baseUIList = new List<BaseUI>();

            public UIParts(Transform _trf, Rect _rect)
            {

            }
            public void Add(BaseUI _ui)
            {
                baseUIList.Add(_ui);
                _ui.transform.SetParent(root, false);
            }
            public T Get<T>() where T : BaseUI
            {
                foreach (var ui in baseUIList)
                {
                    if (ui is T)
                    {
                        return ui as T;
                    }
                }
                return null;
            }
            public void Remove(BaseUI _ui) 
            { 

            }
        }

        // UI 영역을 나타내는 열거형
        public enum UIArea
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        // UI 영역을 관리하기 위한 딕셔너리
        private Dictionary<UIArea, UIParts> uiPartsMap = new Dictionary<UIArea, UIParts>();

        // Start 메서드에서 UI를 초기화합니다.
        private void Start()
        {
            
        }
        /// <summary>
        /// anchorMin = Vector2.zero: 좌상단 앵커를 (0, 0)으로 설정하여 캔버스의 좌측 상단에 정확하게 위치합니다.
        /// anchorMax = new Vector2(0.5f, 0.5f) : 우하단 앵커를(0.5, 0.5)으로 설정하여 캔버스의 가로 길이의 절반, 세로 길이의 절반 위치까지 영역을 확장합니다.
        /// </summary>
        private void CreateUIAreas()
        {
            // 캔버스를 찾습니다.
            Canvas canvas = GetComponentInParent<Canvas>();

            // 캔버스 크기를 가져옵니다.
            Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

            foreach (UIArea area in System.Enum.GetValues(typeof(UIArea)))
            {
                // 분할 영역을 나타내는 빈 오브젝트를 생성합니다.
                GameObject areaObject = new GameObject(area.ToString());
                RectTransform areaRect = areaObject.AddComponent<RectTransform>();
                areaRect.SetParent(canvas.transform, false);

                // 각 분할 영역의 위치와 크기를 설정합니다.
                switch (area)
                {
                    case UIArea.TopLeft:
                        areaRect.anchorMin = Vector2.zero;
                        areaRect.anchorMax = new Vector2(0.5f, 0.5f);
                        areaRect.sizeDelta = new Vector2(canvasSize.x / 2, canvasSize.y / 2);
                        break;
                    case UIArea.TopRight:
                        areaRect.anchorMin = new Vector2(0.5f, 0);
                        areaRect.anchorMax = new Vector2(1, 0.5f);
                        areaRect.sizeDelta = new Vector2(canvasSize.x / 2, canvasSize.y / 2);
                        break;
                    case UIArea.BottomLeft:
                        areaRect.anchorMin = new Vector2(0, 0.5f);
                        areaRect.anchorMax = new Vector2(0.5f, 1);
                        areaRect.sizeDelta = new Vector2(canvasSize.x / 2, canvasSize.y / 2);
                        break;
                    case UIArea.BottomRight:
                        areaRect.anchorMin = new Vector2(0.5f, 0.5f);
                        areaRect.anchorMax = Vector2.one;
                        areaRect.sizeDelta = new Vector2(canvasSize.x / 2, canvasSize.y / 2);
                        break;
                }

                // 피봇을 중앙으로 설정합니다.
                areaRect.pivot = new Vector2(0.5f, 0.5f);
            }
        }

        // 주어진 UI 영역에 UI를 추가하는 메서드
        public void AddUI(UIArea area, BaseUI ui)
        {
            if (uiPartsMap.ContainsKey(area))
            {
                uiPartsMap[area].Add(ui);
            }
            else
            {
                Debug.LogWarning("UIArea " + area + " is not defined.");
            }
        }

        // 주어진 UI 영역에서 UI를 제거하는 메서드
        public void RemoveUI(UIArea area, BaseUI ui)
        {
            if (uiPartsMap.ContainsKey(area))
            {
                uiPartsMap[area].Remove(ui);
            }
            else
            {
                Debug.LogWarning("UIArea " + area + " is not defined.");
            }
        }

        // TODO: 다른 필요한 메서드 및 기능 추가

        public virtual void Show()
        {

        }

        public virtual void Hide() 
        { 

        }
    }
}
