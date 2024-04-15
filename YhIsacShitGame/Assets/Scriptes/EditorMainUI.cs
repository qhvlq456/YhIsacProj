using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YhProj.Game.UI;


namespace YhProj.Game.YhEditor
{
    /// <summary>
    /// eidtor ui들을 버튼에 따라 생성 또는 제거하는 main ui
    /// </summary>
    public class EditorMainUI : MainUI
    {
        [System.Serializable]
        private struct sEditorMainBtn
        {
            public EditorType editorType;
            public Button btn;
            public TextMeshProUGUI text;
        }

        [SerializeField]
        private TextMeshProUGUI curEditorModeText;

        private EditorType curEditorType;

        [SerializeField]
        private List<sEditorMainBtn> editorMainBtnList = new List<sEditorMainBtn>();

        [SerializeField]
        private Button startBtn;

        [SerializeField]
        private Button exitBtn;

        public override void Show(UIInfo _uiInfo)
        {
            startBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();


            foreach(sEditorMainBtn btn in editorMainBtnList)
            {
                btn.btn.onClick.RemoveAllListeners();

                btn.btn.onClick.AddListener(() => EditorBtnClick((int)btn.editorType));
            }

            base.Show(_uiInfo);
        }
        public override void Hide()
        {
            base.Hide();
        }
        private void EditorBtnClick(int _idx)
        {
            string uiName = EditorManager.Instance.uiNameDic[curEditorType];

            EditorManager.Instance.UIManager.HideUI(uiName);

            curEditorType = (EditorType)_idx;

            curEditorModeText.text = string.Format("{0} Tool Mode", curEditorType);

            uiName = EditorManager.Instance.uiNameDic[curEditorType];

            EditorManager.Instance.UIManager.ShowUI(uiName);
        }

    }
}
