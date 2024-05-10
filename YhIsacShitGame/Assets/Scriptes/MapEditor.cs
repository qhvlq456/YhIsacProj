using OpenCover.Framework.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj.Game.Map;

namespace YhProj.Game.YhEditor
{
    // 여기서 타일과 stage 두개를 생각하면 될듯..?
    // 또 두개 다 분리 stagedata라고 생각해야 함
    public class MapEditor : BaseEditor
    {
        public MapEditor()
        {
            // 후에 인터페이스를 이용한 다향성으로 스위칭
            factory = new TileFactory();

            dataHandlerMap.Add(typeof(StageHandler), new StageHandler());
            dataHandlerMap.Add(typeof(TileHandler), new TileHandler());
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
            TileHandler handler = GetDataHandler<TileHandler>();

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
                TileData tileData = handler.GetData<TileData>(list[i]);

                switch (tileData.elementType)
                {
                    case ElementType.mine:
                        tileObject = factory.Create<HeroTileObject>(tileData);
                        break;
                    case ElementType.enemy:
                        tileObject = factory.Create<EnemyTileObject>(tileData);
                        break;
                    case ElementType.deco:
                        tileObject = factory.Create<DecoTileObject>(tileData);
                        break;
                    default:
                        tileObject = factory.Create<EditorTileObject>(tileData);
                        break;
                }

                tileObject.transform.SetParent(root);
                tileObject.transform.localPosition = new Vector3(x, 0, z);
                objectList.Add(tileObject);
                log += $"idx = {tileData.index}, road type = {tileData.elementType}, \n";
            }

            Debug.LogError(log);
        }

        public override void Delete(GameData _gameData)
        {
            if(_gameData is TileData tileData)
            {
                TileObject tileObject = objectList.Select(x => x as TileObject).Where(x => x.tileData.index == _gameData.index).First();
                tileObject?.Delete();
            }
            else
            {

            }
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
