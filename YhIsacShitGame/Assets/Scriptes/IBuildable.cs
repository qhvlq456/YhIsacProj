using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YhProj.Game.Map
{
    public interface IBuildable
    {
        public bool IsBuildable(Vector3Int _startPosition, Vector2Int _size);
    }
}
