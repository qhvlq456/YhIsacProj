using OpenCover.Framework.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj.Game.Map;
using YhProj.Game.Play;

namespace YhProj.Game.YhEditor
{
    // 여기서 타일과 stage 두개를 생각하면 될듯..?
    // 또 두개 다 분리 stagedata라고 생각해야 함
    public class MapEditor : BaseEditor
    {
        private List<TileObject> tileObjectList = new List<TileObject>();
        private ITileFactory tileFactory;
        public MapEditor()
        {
            // 후에 인터페이스를 이용한 다향성으로 스위칭
            // tileFactory = new TileFactory();
        }
        
        public override void Initialize()
        {
            BoxCollider bottomColider = GameUtil.AttachObj<BoxCollider>("Bottom");
            bottomColider.size = new Vector3(100, 0, 100);
        }
        public override void Update()
        {
            
        }

        #region Tile Load and Delete and Save
        /// <summary>
        /// row와 col은 무조건 존재해야 한다
        /// </summary>
        /// <param name="_gameData">stage data </param>
        public override void Create(GameData _gameData)
        {
            TileDataHandler handler = EditorManager.Instance.tileDataHandler;

            StageData stageData = _gameData as StageData;

            string log = "";

            List<int> list = stageData.tileIdxList;

            int tileCount = stageData.row * stageData.col;
            
            for (int i = 0; i < tileCount; i++)
            {
                // 이거 생각해봐야 함
                int z = i / stageData.row;
                int x = i % stageData.col;

                TileObject tileObject = null;
                TileData tileData = handler.GetData(list[i]);

                switch (tileData.elementType)
                {
                    default:
                        tileFactory = new EditorTileFactory();
                        tileObject = tileFactory.Create(tileData);
                        break;
                }

                tileObject.transform.SetParent(root);
                tileObject.transform.localPosition = new Vector3(x, 0, z);
                tileObjectList.Add(tileObject);
                log += $"idx = {tileData.index}, road type = {tileData.elementType}, \n";
            }

            Debug.LogError(log);
        }

        public override void Delete(GameData _gameData)
        {
            if(_gameData is TileData tileData)
            {
                TileObject tileObject = tileObjectList.Where(x => x.tileData.index == _gameData.index).First();
                tileObject?.Delete();
            }
            else
            {

            }
        }
        #endregion

        public override void Dispose()
        {
            
        }

    }
}
