using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game
{
    public abstract class BaseController<T> where T : BaseManager
    {
        protected T manager;
        public BaseController() { }
        public BaseController(BaseManager _manager)
        {
            if (_manager is T)
            {
                manager = _manager as T;
            }
        }

        public virtual void Load()
        {
            // 로드 동작
        }

        public virtual void Update()
        {
            // 업데이트 동작
        }

        public virtual void Delete()
        {
            // 삭제 동작
        }

    }
}
