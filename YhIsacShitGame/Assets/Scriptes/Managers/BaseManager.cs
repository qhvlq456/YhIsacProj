using System;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game
{
    /*
     * json에서는 렌더링 등 필요한 데이터를 받아야 하며
     * server에서는 정보를 받아야 함 즉, load and save
     * 
     * 생각을 하긴 하여야 함 basemanager에서 load와 delete가 필요 한 것인가?
     * 
     * type에 따른 load 분기 update, delete 분기 등등 조정
     */
    public abstract class BaseManager
    {
        protected Dictionary<Type, BaseDataHandler> dataHandlerMap = new Dictionary<Type, BaseDataHandler>();
        public T GetDataHandler<T>() where T : BaseDataHandler, new()
        {
            Type type = typeof(T);

            if (dataHandlerMap.ContainsKey(type))
            {
                return dataHandlerMap[type] as T;
            }
            else
            {
                T newHandler = new T();
                dataHandlerMap[type] = newHandler;
                newHandler.LoadJsonData();
                return newHandler;
            }
        }

        public BaseDataHandler GetDataHandler(Type _type) => dataHandlerMap.TryGetValue(_type, out BaseDataHandler handler) ? handler : null;

        public Transform root { get; protected set; }
        // data의 load 플로우들을 정의
        public abstract void Load();
        // data의 저장 등등 정의
        public abstract void Update();
        // data의 unload 플로우들을 정의
        public abstract void Dispose();

    }
}
