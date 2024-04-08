using UnityEngine;
using YhProj.Game.Map;

namespace YhProj.Game.YhEditor
{
    public class MapEditor : BaseEditor, IDataHandler
    {
        private Transform root;
        private StageHandler stageHandler;
        private EditorTileController editorTileController;
        public override void Initialize()
        {
            BoxCollider bottomColider = GameUtil.AttachObj<BoxCollider>("Bottom");
            bottomColider.size = new Vector3(100, 0, 100);
        }
        public void DataLoad()
        {
            stageHandler.DataLoad();
        }
        public void DataSave<T>(params T[] _params) where T : BaseData
        {
            stageHandler.DataSave(_params);
        }

        #region Tile Load and Delete and Save
        public override void Update()
        {
            
        }

        public override void Create(GameData _gameData)
        {
            StageData stageData = stageHandler.GetStageData(_gameData.index);
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

                    EditorTileObject editorTileObject = editorTileController.LoadTile(tileData);
                    editorTileObject.transform.SetParent(root);
                    editorTileObject.transform.localPosition = new Vector3(x, 0, z);
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
                editorTileController.DeleteTile(tileData);
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
