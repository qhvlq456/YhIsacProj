using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game;
using YhProj.Game.Map;
using YhProj.Game.State;

namespace YhProj.Game.YhEditor
{
    public class EditorManager : BaseManager
    {
        [SerializeField]
        private Transform root;

        [SerializeField]
        private Transform lookTrf;

        [SerializeField]
        private enum EditorType
        {
            Map,
            Character,
        }

        [SerializeField]
        private EditorType editorType;

        private BaseEditor baseEditor;
        private StateController stateController;
        public override void Load()
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

        public override void Update()
        {
            baseEditor?.Update();
            stateController?.Update();
        }

        public override void Dispose()
        {
            baseEditor?.Dispose();
            stateController?.Dispose();
        }
    }
}
