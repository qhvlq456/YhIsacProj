using System.Collections;
using System.Collections.Generic;
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
            EDITOR,
            ANDROID,
            IOS
        }
        // 모든 게임 데이터의 베이스의 타입 // base type -> subType 으로 간다
        public enum BaseType
        {
            NONE,
            TILE,
            CHARACTER,
            BUILD,
            ITEM,
            UI
        }
        // manager type 등 정의
        public enum ManagerType
        {
            TILE,
            BUILD,
            CHARACTER,
            ITEM
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
            MAIN_UI,
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
            BOTTOM
        }
    }

    /// <summary>
    /// 지정된 맥스 레벨 값 하지만 임시용임 나중에 set할 것 임
    /// </summary>
    public class StaticDefine
    {
        // 임시용 데이터 저장용
        public static int USER_LEVEL = 1;
        public static int MAX_LEVEL = 10;

        public static int MAX_CREATE_TILE_NUM = 50;

        public static string JSON_MAP_FILE_NAME = "StageData.json";


        // json 위치 
        public static string JSON_MAP_DATA_PATH = "StreamingAssets";


        // 리소스 위치
        public static string TILE_PATH = "";

        // 임시 일단 넣을거
        public static float TILE_YPOSITION = 0f;

        // executionDataPath // scriptableobject 실행 환경 오브젝트
        public static string EXECUTIONDATA_PATH = "Assets/Editor/ExecutionData.asset";

        public static UnityEngine.Vector3 START_POSITION = UnityEngine.Vector3.zero;

    }
}
