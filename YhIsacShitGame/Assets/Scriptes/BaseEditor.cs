using System.Collections;
namespace YhProj.Game.YhEditor
{
    public abstract class BaseEditor
    {
        public abstract void Initialize();
        // data 필요 
        public abstract void Create(GameData _gameData);

        public abstract void Delete(GameData _gameData);
        public abstract void Dispose();
        
    }
}
