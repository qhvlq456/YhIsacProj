using System;

namespace YhProj
{
    [Serializable]
    public class Define
    {
        // ���� ����� � ���� ���� ������ Ÿ��
        public enum GameMode
        {
            TEST,
            EDITOR,
            MAPTOOL,
            BUILDTOOL
        }
        // ��� ���� �������� ���̽��� Ÿ�� // base type -> subType ���� ����
        public enum BaseType
        {
            NONE,
            TILE,
            CHARACTER,
            BUILD,
            ITEM,
            UI
        }
        // log�� ���̰� �� ������ �Ⱥ��̰� �� ������
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
        // uiroot ���� �������� ĵ���� ������
        public enum UIRootType
        {
            MAIN_UI,
            POPUP_UI,
            TOOLTIP_UI,
            CONTEXTUAL_UI,
            TEST_UI, // �׽�Ʈ�� �׻� �Ʒ� (������ ���� �������)
            COUNT
        }

        // define symbol �� enumó��
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
            LEFT_TOP, // �»�
            RIGHT_TOP, // ���
            LEFT_BOTTOM, // ����
            RIGHT_BOTTOM // ����
        }


        // input
        public enum InputStateType
        {
            NONE,
            SELECT,
            DRAG,
            MOVE,
        }
    }

    /// <summary>
    /// ������ �ƽ� ���� �� ������ �ӽÿ��� ���߿� set�� �� ��
    /// </summary>
    public class StaticDefine
    {
        // �ӽÿ� ������ �����
        public static int USER_LEVEL = 1;
        public static int MAX_LEVEL = 10;

        public static int MAX_CREATE_TILE_NUM = 12;

        public static string JSON_MAP_FILE_NAME = "StageData.json";


        // json ��ġ 
        public static string JSON_MAP_DATA_PATH = "StreamingAssets";


        // ���ҽ� ��ġ
        public static string TILE_PATH = "";

        // �ӽ� �ϴ� ������
        public static float TILE_YPOSITION = 0f;

        // executionDataPath // scriptableobject ���� ȯ�� ������Ʈ // �ϴ� ������ ������ ���� ������ �߻��ϱ� �ؼ� �ӽ÷� ������ ������ ����
        public static string EXECUTIONDATA_PATH = "Assets/Editor/ExecutionData.asset";

        public static UnityEngine.Vector3 START_POSITION = UnityEngine.Vector3.zero;

    }
}
