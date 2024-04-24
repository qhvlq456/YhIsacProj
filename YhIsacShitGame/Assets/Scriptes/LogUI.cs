using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YhProj.Game.UI;

namespace YhProj.Game.Log
{
    public class LogUI : BaseUI
    {
        [Header("Log Text")]
        [SerializeField]
        private TextMeshProUGUI logText;
        private RectTransform logTextRect;
        private ContentSizeFitter contentSizeFitter;

        [Header("Scroll Rect")]
        [SerializeField]
        private ScrollRect scrollRect;

        private void Awake()
        {
            logTextRect = logText.GetComponent<RectTransform>();
            contentSizeFitter = logText.GetComponent<ContentSizeFitter>();
        }
        
        public override void Show(UIInfo _uiInfo)
        {
            base.Show(_uiInfo);
        }
        // 후에 에러 타입별로 format 변경 되겠금 생각해야 함 log에 대한 클래스 구현 이후
        public void EnterLog(string _log)
        {
            logText.text += string.Format("\n{0}", _log);
            // ContentSizeFitter 구성 요소에 의해 다음 프레임에서 계산될 수도 있습니다.
            // Unity 문서를 확인한 후 시스템이 SetLayoutHorizontal 메서드를 호출하여 게임 개체의 크기를 자동으로 조정한다는 사실을 발견했습니다.
            // 그래서 해당 메소드를 호출하여 게임오브젝트의 크기를 즉시 조정하도록 강제합니다. 그런 다음 문제가 해결되었습니다.
            contentSizeFitter.SetLayoutVertical();
            UpdateRect();
        }

        private void UpdateRect()
        {
            //float scrollYSize = scrollRect.GetComponent<RectTransform>().sizeDelta.y;
            //// yOffset = content heigth - content pos y 
            //// scrollYSize
            //float textYSize = logTextRect.sizeDelta.y;

            //float offset = textYSize - scrollYSize;

            //if (textYSize > scrollYSize)
            //{
            //    // left, bottom
            //    scrollRect.content.offsetMin = new Vector2(scrollRect.content.offsetMin.x, scrollRect.content.offsetMin.y);
            //    // rigth, top
            //    scrollRect.content.offsetMax = new Vector2(scrollRect.content.offsetMax.x, offset);
            //}

            float contentHeight = logTextRect.rect.height;
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, contentHeight);

            // verticalNormalizedPosition 1 이면 top, 0이면 bottom임
            scrollRect.verticalNormalizedPosition = 0f; // 스크롤을 맨 아래로 이동합니다.

        }
        public override void Hide()
        {
            base.Hide();
        }




        // 후에 특정 scroll item 위치로 이동할 때 사용 할 코드
        public RectTransform itemToScrollTo;

        public void ScrollToItem()
        {
            // 아이템의 위치를 가져옵니다.
            Vector3 itemPosition = itemToScrollTo.position;

            // Scroll Rect의 Content 영역의 크기를 가져옵니다.
            Vector3 contentSize = scrollRect.content.rect.size;

            //scrollview: 800 이면 이값도 800임
            // Scroll Rect의 Viewport 영역의 크기를 가져옵니다.
            Vector3 viewportSize = ((RectTransform)scrollRect.viewport).rect.size;

            // 아이템이 Content의 가운데에 위치하도록 합니다.
            Vector3 targetPosition = itemPosition - contentSize / 2f + viewportSize / 2f;

            // Scroll Rect의 Content 영역을 이동합니다.
            scrollRect.content.localPosition = -targetPosition;

            // Scroll Rect의 normalized position을 다시 계산하여 설정합니다.
            Vector2 normalizedPosition = new Vector2(
                targetPosition.x / (contentSize.x - viewportSize.x),
                targetPosition.y / (contentSize.y - viewportSize.y)
            );

            // normalized position이 유효한 범위에 있도록 클램핑합니다.
            normalizedPosition = new Vector2(
                Mathf.Clamp01(normalizedPosition.x),
                Mathf.Clamp01(normalizedPosition.y)
            );

            // Scroll Rect의 normalized position을 설정합니다.
            scrollRect.normalizedPosition = normalizedPosition;
        }

    }


    

    public class ScrollToItem : MonoBehaviour
    {
        [SerializeField] private RectTransform content; // Scroll View의 Content Transform
        [SerializeField] private RectTransform[] items; // Content 내에 있는 Item들의 RectTransform 배열
        [SerializeField] private ScrollRect scrollRect; // Scroll View의 ScrollRect 컴포넌트

        // Scroll View를 주어진 Item의 위치로 스크롤하는 함수
        public void ScrollToItemIndex(int index)
        {
            // 만약 인덱스가 유효한 범위 내에 있을 경우에만 실행
            if (index >= 0 && index < items.Length)
            {
                // 주어진 인덱스에 해당하는 Item의 위치를 가져옴
                Vector3 targetPosition = items[index].localPosition;

                // Content의 위치를 조정하여 주어진 Item이 스크롤 영역에 보이도록 함
                content.localPosition = CalculateContentPosition(targetPosition);

                // ScrollRect의 값을 변경하여 스크롤 위치를 업데이트함
                UpdateScrollRect();
            }
        }

        //Content의 위치 조정:
        //        targetPosition: 스크롤 영역에 보이도록 할 대상 아이템의 위치입니다.
        //        scrollSize: 스크롤 뷰의 크기입니다.
        //        pivot: Content의 Pivot 지점을 나타냅니다.
        //        newPosition: 대상 아이템이 스크롤 영역에 보이도록 Content의 새로운 위치를 계산합니다.
        //        scrollSize.x* pivot.x, scrollSize.y* pivot.y: Content의 Pivot을 고려하여 스크롤 영역 내에서의 위치를 조정하기 위해 사용됩니다.
        //Content 위치 제한:
        //contentSize: Content의 크기입니다.
        //Mathf.Clamp(): 값을 주어진 최소값과 최대값 사이로 제한합니다.
        //newPosition.x: Content의 x축 위치를 최대 -contentSize.x + scrollSize.x에서 최소 0까지 제한합니다. 이는 Content가 왼쪽 끝까지 스크롤되지 않도록 합니다.
        //        newPosition.y: Content의 y축 위치를 최대 -contentSize.y + scrollSize.y에서 최소 0까지 제한합니다. 이는 Content가 아래쪽 끝까지 스크롤되지 않도록 합니다.
        // Content의 위치를 조정하여 주어진 Item이 스크롤 영역에 보이도록 함
        private Vector3 CalculateContentPosition(Vector3 targetPosition)
        {
            // Content의 Pivot 값을 가져옴
            Vector2 pivot = content.pivot;

            // Scroll View의 크기를 가져옴
            Vector2 scrollSize = scrollRect.viewport.rect.size;

            // Content의 크기를 가져옴
            Vector2 contentSize = content.rect.size;

            // Content의 위치를 조정하여 주어진 Item이 스크롤 영역에 보이도록 함
            Vector3 newPosition = targetPosition - new Vector3(scrollSize.x * pivot.x, scrollSize.y * pivot.y, 0f);

            // 아 내려가야하니깐(-) 더해야(+) 하는구나
            // Content가 끝까지 스크롤되지 않도록 위치를 제한함
            newPosition.x = Mathf.Clamp(newPosition.x, -contentSize.x + scrollSize.x, 0f);
            newPosition.y = Mathf.Clamp(newPosition.y, -contentSize.y + scrollSize.y, 0f);

            return newPosition;
        }

        // ScrollRect의 값을 변경하여 스크롤 위치를 업데이트함
        private void UpdateScrollRect()
        {
            // 수직 스크롤의 위치를 맨 위로 이동시키기 위해 y 값에 1을 사용합니다.
            // x 값은 수평 스크롤이므로 0을 사용합니다. 따라서 (0, 1)은 스크롤 뷰를 맨 위로 초기화하는 것을 의미합니다.
            // ScrollRect를 갱신하여 스크롤 위치를 업데이트함
            scrollRect.normalizedPosition = new Vector2(0f, 1f);
        }
    }


}
