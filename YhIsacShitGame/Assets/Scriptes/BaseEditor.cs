using System.Collections.Generic;
using System;
using UnityEngine;


namespace YhProj.Game.YhEditor
{
    public abstract class BaseEditor
    {
        protected Transform root;

        //protected IFactory factory;
        public abstract void Initialize();
        // data 필요 
        public abstract void Create(GameData _gameData);

        public abstract void Delete(GameData _gameData);
        public abstract void Dispose();
        public abstract void Update();
    }
}
