namespace YhProj
{
    /*
     * json에서는 렌더링 등 필요한 데이터를 받아야 하며
     * server에서는 정보를 받아야 함 즉, load and save
     * 
     * 생각을 하긴 하여야 함 basemanager에서 load와 delete가 필요 한 것인가?
     */
    public abstract class BaseManager : ILogger
    {
        public Define.ManagerType type { protected set; get; }

        // data의 load 플로우들을 정의
        public abstract void Load();
        // data의 저장 등등 정의
        public abstract void Update();
        // data의 unload 플로우들을 정의
        public abstract void Delete();
        public virtual void Logger()
        {
            UnityEngine.Debug.LogWarningFormat("BaseManager \n type : {0}", type);
        }
    }
}
