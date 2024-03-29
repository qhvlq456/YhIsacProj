using System;

namespace YhProj
{
    [Serializable]
    public class Define
    {
        // 게임 실행시 어떤 모드로 선택 할지의 타입
        public enum GameMode
        {
            TEST,
            Build,
        }
        // log를 보이게 할 것인지 안보이게 할 것인지
        public enum DebugLogeer
        {
            ENABLE,
            DISABLE
        }

        public enum ServerType
        {
            DEV,
            ALHPA,
            LIVE
        }
        // uiroot 하위 랜더링할 캔버스 종류들
        public enum UIRootType
        {
            MAIN_UI, // 항상 고정값이 되어야 함
            POPUP_UI,
            TOOLTIP_UI,
            CONTEXTUAL_UI,
            TEST_UI, // 테스트는 항상 아래 (솔직히 순서 상관없음)
            COUNT
        }

        // define symbol 들 enum처리
        public enum DefineSymbol
        {
            TEST1,
            TEST2
        }

        public enum Direction
        {
            LEFT,
            RIGHT,
            TOP,
            BOTTOM,
            LEFT_TOP, // 좌상
            RIGHT_TOP, // 우상
            LEFT_BOTTOM, // 좌하
            RIGHT_BOTTOM, // 우하
        }
    }

    // 나중에 경로에 대한 지정이 다시 필요할듯
    /// <summary>
    /// 지정된 맥스 레벨 값 하지만 임시용임 나중에 set할 것 임
    /// </summary>
    public class StaticDefine
    {
        // 임시용 데이터 저장용
        public static int USER_LEVEL = 1;
        public static int MAX_LEVEL = 10;

        public static int MAX_CREATE_TILE_NUM = 12;

        public static string JSON_MAP_FILE_NAME = "StageData.json";

        public static string json_character_file_name = "hero.json";
        public static string json_enemy_file_name = "Enemy.json";

        // json 위치 
        public static string json_data_path = "StreamingAssets";

        // 임시 일단 넣을거
        public static float TILE_YPOSITION = 0f;

        // executionDataPath // scriptableobject 실행 환경 오브젝트 // 일단 에디터 폴더에 없음 에러가 발생하긴 해서 임시로 에디터 폴더로 지정
        public static string SCRIPTABLEOBJECT_PATH = "Assets/Resources/ScriptableObjects/";

        public static UnityEngine.Vector3 START_POSITION = UnityEngine.Vector3.zero;

    }
}
