using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.State;
using YhProj.Game.UI;

namespace YhProj.Game.YhEditor
{
    public enum EditorType
    {
        None = 0,
        Map,
        Character,
    }

    // 에디터 주체 클래스 현제 mapTool, characterTool 관련 기능을 담당한다 .. stagetool도 만들어야 되나? maptool이랑 다른게 뭐지?
    // stage map을 두개를 나누어야 하는데 (stage,tile) 너무 깊숙히 와버렸고 stage는 감도 안잡히네
    public class EditorManager : Singleton<EditorManager>
    {
        public readonly Dictionary<EditorType, string> uiNameDic = new Dictionary<EditorType, string>()
        {
            { EditorType.Map, "MapToolUI"},
            { EditorType.Character, "CharacterToolUI"}
        };

        [SerializeField]
        private Transform root;

        [SerializeField]
        private Transform lookTrf;

        [SerializeField]
        private EditorType editorType;

        private UIManager uiManager;

        public UIManager UIManager
        {
            get
            {
                if(uiManager == null) 
                {
                    uiManager = Managers.Instance.GetManager<UIManager>();
                    uiManager.Load();
                }

                return uiManager;
            }
        }

        private BaseEditor baseEditor;
        private StateController stateController;
        
        public IDataHandler<T> GetDataHandler<T>() where T : GameData
        {
            return baseEditor.GetHandler<T>();
        }
        protected override void Awake()
        {
            base.Awake();

            stateController?.Initialize();
        }
        
        public void ChangeEditor(EditorType _editorType)
        {
            editorType = _editorType;

            switch (editorType) 
            {
                case EditorType.Map:
                    baseEditor = new MapEditor();
                    stateController = new EditStateController(lookTrf);
                    break;
                case EditorType.Character:
                    break;
            }
        }
        public void Create(GameData _gameData)
        {
            baseEditor?.Create(_gameData);
        }
        public void Delete(GameData _gameData)
        {
            baseEditor?.Delete(_gameData);
        }

        public void Update()
        {
            baseEditor?.Update();
            stateController?.Update();
        }
        public void Dispose()
        {
            baseEditor?.Dispose();
            stateController?.Dispose();
        }

        public void Save<T>(params T[] data) where T : GameData
        {
            IDataHandler<T> handler = GetDataHandler<T>();
            handler?.SaveData(data);
        }
    }
}
