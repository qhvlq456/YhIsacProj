using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using YhProj.Game.Map;
using YhProj.Game.Play;

namespace YhProj.Game.Character
{
    public class CharacterManager : BaseManager, IGameFlow
    {
        private List<CharacterObject> instanceCharList = new List<CharacterObject>();
        private IFactorySelector factorySelector;
        public override void Load()
        {
            if (root == null)
            {
                root = GameUtil.AttachObj<Transform>("CharaterRoot");
                root.transform.position = Vector3.zero;
            }

            // 아님 나중에 basemanager로 load부분 gameadta load하는 부분으로 통합 작업 필요
            // 나중에 각 매니저마다 비동기식으로 로드 할 것
            dataHandlerMap.Add(typeof(CharDataHandler), new CharDataHandler());

            foreach (var item in dataHandlerMap)
            {
                item.Value.LoadJsonData();
            }
        }
        public override void Update()
        {
            
        }
        public override void Dispose()
        {
            
        }

        // 시작 지점을 알긴 알아야하는데 의존성 때문에;; 매니저가 알 필욘 없긴 해
        public void OnStart()
        {
            StageData stageData = GameManager.Instance.stageData;
            CharDataHandler handler = GetDataHandler<CharDataHandler>();

            string log = "";

            List<int> list = stageData.enemyIdxList;

            for (int i = 0; i < list.Count; i++)
            {
                CharacterData charData = handler.GetData<CharacterData>(list[i]);

                if (charData != null)
                {
                    ICharacterFactory characterFactory = factorySelector.SelectFactory(charData.elementType);
                    if (characterFactory != null)
                    {
                        CharacterObject charObject = characterFactory.Create(charData);
                        charObject.transform.SetParent(root);
                        charObject.transform.localPosition = Vector3.zero;
                        instanceCharList.Add(charObject);
                    }
                    else
                    {
                        Debug.LogError($"No factory found for ElementType: {charData.elementType}");
                    }
                }
                else
                {
                    Debug.LogError($"CharacterData not found for index: {list[i]}");
                }
            }
        }

        public void OnUpdate()
        {
            foreach (var item in instanceCharList)
            {
                item.Update();
            }
        }

        public void OnEnd()
        {
            foreach(var item in instanceCharList)
            {
                item?.Delete();
            }
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

            List<HeroData> heroTileDataList = GameUtil.LoadJsonArray<HeroData>(StaticDefine.json_data_path, json_char_hero_file_name);
            List<EnemyData> enemyTileDataList = GameUtil.LoadJsonArray<EnemyData>(StaticDefine.json_data_path, json_enemy_hero_file_name);

            list.AddRange(heroTileDataList);
            list.AddRange(enemyTileDataList);

            foreach (var data in list)
            {
                if (!dataMap.ContainsKey(data.index))
                {
                    dataMap.Add(data.index, data);
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
