using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace YhProj.Game.Map
{
    // 이것도 하나의 state로 놔야 할 것 같은데 ㅋㅋ
    public class GridHandler
    {
        private readonly string path;

        [SerializeField]
        private int row;
        [SerializeField]
        private int col;

        private Transform root;
        private Transform target;

        private List<GridObject> gridObjectList = new List<GridObject>();

        public GridHandler(Transform _target)
        {
            target = _target;

            int createCount = row + col;
            for(int i = 0; i < createCount; i++) 
            {
                GridObject gridObject = GameUtil.InstantiateResource<GridObject>(path);

            }
        }
        /// <summary>
        /// gird를 생성하고 이동시키는 함수
        /// </summary>
        /// <param name="_baseObject"> grid를 표현할 오브젝트 건물</param>
        public void Move(BaseObject _baseObject)
        {

        }

        private bool IsBuild()
        {
            bool ret = false;

            return ret;
        }

    }
}
