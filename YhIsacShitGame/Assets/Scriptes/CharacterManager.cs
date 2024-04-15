using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace YhProj.Game.Character
{
    public class CharacterManager : BaseManager, IDataHandler
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

        public void DataLoad()
        {
            List<HeroData> heroDataList = GameUtil.LoadJsonArray<HeroData>(StaticDefine.json_data_path, StaticDefine.json_character_file_name);
            heroDataDic = heroDataList.ToDictionary(k => k.index, v => v);

            List<EnemyData> enemyDataList = GameUtil.LoadJsonArray<EnemyData>(StaticDefine.json_data_path, StaticDefine.json_enemy_file_name);
            enemyDataDic = enemyDataList.ToDictionary(k => k.index, v => v);
        }

        public void DataSave<T>(params T[] _params) where T : GameData
        {
            
        }
    }
}
