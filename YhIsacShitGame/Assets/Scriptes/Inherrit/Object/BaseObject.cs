using UnityEngine;
using UnityEngine.EventSystems;

namespace YhProj.Game
{
    /// <summary>
    /// 모든 field의 부모 object 클래스
    /// </summary>
    public abstract class BaseObject : MonoBehaviour
    {
        public GameData gameData;
        public T GetData<T>() where T : GameData
        {
            return gameData as T;
        }
        
        public abstract void Create<T>(T _data);

        public abstract void Delete();

        public virtual void Update() { }
    }
}
