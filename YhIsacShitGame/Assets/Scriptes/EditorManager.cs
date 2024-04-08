using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game;

namespace YhProj.Game.YhEditor
{
    public class EditorManager : BaseManager
    {
        private BaseEditor baesEditor;

        public override void Load(Define.GameMode _gameMode)
        {
            IDataHandler dataHandler = baesEditor as IDataHandler;

            if(dataHandler != null) 
            {
                dataHandler.DataLoad();
            }
        }
        public void Save()
        {
            IDataHandler dataHandler = baesEditor as IDataHandler;

            if (dataHandler != null)
            {
                //dataHandler.DataSave();
            }
        }
        public override void Update()
        {
            throw new System.NotImplementedException();
        }
        public override void Delete()
        {
            baesEditor.Dispose();
        }
    }
}
