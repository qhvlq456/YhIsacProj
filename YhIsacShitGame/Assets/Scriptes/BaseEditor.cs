using System.Collections.Generic;
using System;
using UnityEngine;


namespace YhProj.Game.YhEditor
{
    public abstract class BaseEditor
    {
        protected Transform root;

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
                return newHandler;
            }
        }

        public BaseDataHandler GetDataHandler(Type _type) => dataHandlerMap.TryGetValue(_type, out BaseDataHandler handler) ? handler : null;


        protected List<BaseObject> objectList = new List<BaseObject>();
        protected IFactory factory;
        public abstract void Initialize();
        // data 필요 
        public abstract void Create(GameData _gameData);

        public abstract void Delete(GameData _gameData);
        public virtual void Dispose()
        {
            foreach (var obj in objectList)
            {
                obj.Delete();
            }

            objectList.Clear();
        }
        public abstract void Update();
    }
}
