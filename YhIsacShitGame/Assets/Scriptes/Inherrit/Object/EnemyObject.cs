using System.Collections.Generic;
using YhProj.Game.Play;
using YhProj.Game.Map;

namespace YhProj.Game.Character
{
    public class EnemyObject : CharacterObject, IMovement
    {
        // 목적지에 타일 리스트
        private List<TileData> destTiles = new List<TileData>();

        public void Fly()
        {
            throw new System.NotImplementedException();
        }

        public void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}

