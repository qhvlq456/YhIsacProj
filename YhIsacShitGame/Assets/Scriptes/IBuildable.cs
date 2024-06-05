using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YhProj.Game.Map
{
    public interface IBuildable
    {
        public bool IsBuildable(Vector3 _position);
    }
}
