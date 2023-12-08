namespace YhProj
{
    /*
     * json������ ������ �� �ʿ��� �����͸� �޾ƾ� �ϸ�
     * server������ ������ �޾ƾ� �� ��, load and save
     * 
     * ������ �ϱ� �Ͽ��� �� basemanager���� load�� delete�� �ʿ� �� ���ΰ�?
     * 
     * type�� ���� load �б� update, delete �б� ��� ����
     */
    public abstract class BaseManager : ILogger
    {
        // data�� load �÷ο���� ����
        public abstract void Load(Define.GameMode _gameMode);
        // data�� ���� ��� ����
        public abstract void Update();
        // data�� unload �÷ο���� ����
        public abstract void Delete();
        public virtual void Logger()
        {
            UnityEngine.Debug.LogWarningFormat("BaseManager");
        }
    }
}
