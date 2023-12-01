namespace YhProj
{
    /*
     * json������ ������ �� �ʿ��� �����͸� �޾ƾ� �ϸ�
     * server������ ������ �޾ƾ� �� ��, load and save
     * 
     * ������ �ϱ� �Ͽ��� �� basemanager���� load�� delete�� �ʿ� �� ���ΰ�?
     */
    public abstract class BaseManager : ILogger
    {
        public Define.ManagerType type { protected set; get; }

        // data�� load �÷ο���� ����
        public abstract void Load();
        // data�� ���� ��� ����
        public abstract void Update();
        // data�� unload �÷ο���� ����
        public abstract void Delete();
        public virtual void Logger()
        {
            UnityEngine.Debug.LogWarningFormat("BaseManager \n type : {0}", type);
        }
    }
}
