using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Map
{
    public class MapEditor : MonoBehaviour
    {
        private Transform root;
        private List<TileObject> tileObjectList = new List<TileObject>();
        private TileFactory tileFactory;

        private void Awake()
        {
            BoxCollider bottomColider = GameUtil.AttachObj<BoxCollider>("Bottom");
            bottomColider.size = new Vector3(100, 0, 100);
        }

        #region Tile Load and Delete and Save
        public void LoadStage(StageData _stageData)
        {
            string log = "";

            for (int i = 0; i < _stageData.Row; i++)
            {
                for (int j = 0; j < _stageData.Col; j++)
                {
                    TileData tileData = new TileData();

                    if (_stageData.tileArr[i, j] != null)
                    {
                        tileData = _stageData.tileArr[i, j];
                    }
                    else
                    {
                        tileData.type = BaseType.TILE;
                        tileData.elementType = ElementType.ENEMY;
                    }

                    tileData.index = i * _stageData.Col + j;
                    tileData.resName = "EditorTileObject";

                    int z = tileData.index / _stageData.Row;
                    int x = tileData.index % _stageData.Col;

                    EditorTileObject editorTileObject = tileFactory.Create<EditorTileObject>(tileData, new Vector3(x, 0, z));

                    tileObjectList.Add(editorTileObject);
                    log += $"idx = {tileData.index}, road type = {tileData.elementType}, ";
                }
                log += '\n';
            }
            Debug.LogError(log);
        }
        public void DeleteStage()
        {
            foreach (var tile in tileObjectList)
            {
                tile.Delete();
            }

            tileObjectList.Clear();
        }
        #endregion
    }
}
