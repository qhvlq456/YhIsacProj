using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Map;

namespace YhProj.Game.YhEditor
{
    public class MapEditor : BaseEditor
    {
        private Transform root;

        private TileFactory tileFactory;

        private List<TileObject> tileObjectList = new List<TileObject>();

        public MapEditor()
        {
            // 후에 인터페이스를 이용한 다향성으로 스위칭
            tileFactory = new TileFactory();
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

        public override void Create(GameData _gameData)
        {
            StageData stageData = _gameData as StageData;

            string log = "";

            for (int i = 0; i < stageData.Row; i++)
            {
                for (int j = 0; j < stageData.Col; j++)
                {
                    TileData tileData = new TileData();

                    if (stageData.tileArr[i, j] != null)
                    {
                        tileData = stageData.tileArr[i, j];
                    }
                    else
                    {
                        tileData.type = BaseType.TILE;
                        tileData.elementType = ElementType.ENEMY;
                    }

                    tileData.index = i * stageData.Col + j;
                    tileData.resName = "EditorTileObject";

                    int z = tileData.index / stageData.Row;
                    int x = tileData.index % stageData.Col;

                    EditorTileObject editorTileObject = tileFactory.Create<EditorTileObject>(tileData);
                    editorTileObject.transform.SetParent(root);
                    editorTileObject.transform.localPosition = new Vector3(x, 0, z);
                    tileObjectList.Add(editorTileObject);
                    log += $"idx = {tileData.index}, road type = {tileData.elementType}, ";
                }
                log += '\n';
            }

            Debug.LogError(log);
        }

        public override void Delete(GameData _gameData)
        {
            if(_gameData is TileData tileData)
            {
                TileObject tileObject = tileObjectList.Find(x => x.tileData == tileData);
                tileObject?.Delete();
            }
            else
            {

            }
        }
        #endregion

        public override void Dispose()
        {
            foreach(var tile in tileObjectList)
            {
                tile.Delete();
            }

            tileObjectList.Clear();
        }
    }
}
