using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using YhProj.Game;
using YhProj.Game.Map;

namespace YhProj.Game.YhEditor
{
    public class EditorManager : BaseManager
    {
        private BaseEditor baseEditor;

        public override void Load(Define.GameMode _gameMode)
        {
            baseEditor.Initialize();

            if (baseEditor is IDataHandler dataHandler)
            {
                dataHandler.DataLoad();
            }
            else
            {

            }
        }
        
        public void Save<T>(params T[] _params) where T : Map.TileData
        {
            if(baseEditor is IDataHandler dataHandler)
            {
                dataHandler.DataSave<Map.TileData>(_params);
            }
            else
            {

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
            baseEditor.Update();
        }
        public override void Dispose()
        {
            baseEditor.Dispose();
        }
    }
}
