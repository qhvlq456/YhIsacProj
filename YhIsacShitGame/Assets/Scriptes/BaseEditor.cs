using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.YhEditor
{
    public abstract class BaseEditor
    {
        protected Transform root;
        protected IFactory factory;
        protected List<BaseObject> objectList = new List<BaseObject>();

        public abstract IDataHandler<T> GetHandler<T>() where T : GameData;
        public abstract void Initialize();
        // data 필요 
        public abstract void Create(GameData _gameData);

        public abstract void Delete(GameData _gameData);
        public abstract void Dispose();
        public abstract void Update();
    }
}
