using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Map;
using YhProj.Game.State;
using YhProj.Game.UI;

namespace YhProj.Game.YhEditor
{
    [SerializeField]
    public enum EditorType
    {
        Map,
        Character,
    }

    // 에디터 주체 클래스
    public class EditorManager : Singleton<EditorManager>
    {
        public readonly Dictionary<EditorType, string> uiNameDic = new Dictionary<EditorType, string>()
        {
            { EditorType.Map, "MapToolMainUI"},
            { EditorType.Character, "CharacterToolMainUI"}
        };

        [SerializeField]
        private Transform root;

        [SerializeField]
        private Transform lookTrf;

        [SerializeField]
        private EditorType editorType;

        private StageHandler stageHandler;
        public StageHandler StageHandler
        {
            get
            {
                return stageHandler;
            }
        }

        private UIManager uiManager;

        public UIManager UIManager
        {
            get
            {
                uiManager = Managers.Instance.GetManager<UIManager>();
                uiManager.Load();
                return uiManager;
            }
        }


        private BaseEditor baseEditor;
        private StateController stateController;

        protected override void Awake()
        {
            base.Awake();

            Load();
        }
        public void Load()
        {
            stateController.Initialize();

            if (baseEditor is IDataHandler dataHandler)
            {
                dataHandler.DataLoad();
            }
            else
            {

            }
        }
        
        public void Save<T>(params T[] _params) where T : TileData
        {
            if (baseEditor is IDataHandler dataHandler)
            {
                dataHandler.DataSave<TileData>(_params);
            }
            else
            {

            }

        }
        public void ChangeEditor()
        {
            switch (editorType) 
            {
                case EditorType.Map:
                    baseEditor = new MapEditor();
                    stageHandler = new StageHandler();
                    break;
                case EditorType.Character:
                    break;
            }
        }
        public void ChangeState()
        {
            switch (editorType)
            {
                case EditorType.Map:
                    stateController = new EditStateController(lookTrf);
                    break;
                case EditorType.Character:
                    break;
            }
        }
        public void Create(GameData _gameData)
        {
            baseEditor.Create(_gameData);
        }
        public void Delete(GameData _gameData)
        {
            baseEditor.Delete(_gameData);
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
    }
}
