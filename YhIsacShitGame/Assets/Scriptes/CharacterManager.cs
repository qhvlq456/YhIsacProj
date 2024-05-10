using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using YhProj.Game.Map;
using Unity.VisualScripting;

namespace YhProj.Game.Character
{
    public class CharacterManager : BaseManager
    {
        private CharDataHandler charDataHandler;

        public override void Load()
        {
            charDataHandler = new CharDataHandler();

            if (root == null)
            {
                root = GameUtil.AttachObj<Transform>("CharaterRoot");
                root.transform.position = Vector3.zero;
            }
        }
        public override void Update()
        {
            
        }
        public override void Dispose()
        {
            
        }
    }

    // Data류 클래스들은 idatahandler를 통해 재구현하고 클래스로 데이터 보관
    public sealed class CharDataHandler : BaseDataHandler
    {
        public static string json_char_hero_file_name = "StageData.json";
        public static string json_enemy_hero_file_name = "StageData.json";
        public override void LoadJsonData()
        {
            List<CharacterData> list = new List<CharacterData>();

            List<HeroTileData> heroTileDataList = GameUtil.LoadJsonArray<HeroTileData>(StaticDefine.json_data_path, json_char_hero_file_name);
            List<EnemyTileData> enemyTileDataList = GameUtil.LoadJsonArray<EnemyTileData>(StaticDefine.json_data_path, json_enemy_hero_file_name);

            list.AddRange(heroTileDataList);
            list.AddRange(enemyTileDataList);

            foreach (var data in list)
            {
                if (!dataDic.ContainsKey(data.index))
                {
                    dataDic.Add(data.index, data);
                }
                else
                {
                    Debug.LogError($"[CharDataHandler] Already is constains key : {data.index}!!");
                }
            }
        }

        public override void SaveJsonData()
        {
            // GameUtil.CreateJsonFile(StaticDefine.json_data_path, StaticDefine.JSON_MAP_FILE_NAME, GetDataList());
        }

        public override void SaveJsonData<T>(params T[] _params)
        {
            List<CharacterData> charList = _params.OfType<CharacterData>().ToList();
        }
    }
}
