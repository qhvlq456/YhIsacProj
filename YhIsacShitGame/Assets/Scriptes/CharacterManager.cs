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
        private Dictionary<int, HeroData> heroDataDic = new Dictionary<int, HeroData>();
        private Dictionary<int, EnemyData> enemyDataDic = new Dictionary<int, EnemyData>();

        private CharacterFactory characterFactory;
        public Transform root { get; private set; }


        public override void Load()
        {
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
    public sealed class CharDataHandler : BaseDataHandler<CharacterData>
    {
        public static string json_char_hero_file_name = "StageData.json";
        public static string json_enemy_hero_file_name = "StageData.json";
        public override void LoadData()
        {
            List<CharacterData> list = new List<CharacterData>();

            List<HeroTileData> heroTileDataList = GameUtil.LoadJsonArray<HeroTileData>(StaticDefine.json_data_path, json_char_hero_file_name);
            List<EnemyTileData> enemyTileDataList = GameUtil.LoadJsonArray<EnemyTileData>(StaticDefine.json_data_path, json_enemy_hero_file_name);

            list.AddRange(heroTileDataList);
            list.AddRange(enemyTileDataList);

            dataDic = list.ToDictionary(k => k.index, v => v);
        }

        public override void SaveData()
        {
            // GameUtil.CreateJsonFile(StaticDefine.json_data_path, StaticDefine.JSON_MAP_FILE_NAME, GetDataList());
        }

        public override void SaveData(params CharacterData[] _params)
        {
            List<CharacterData> stageList = _params.ToList();
        }
    }
}
