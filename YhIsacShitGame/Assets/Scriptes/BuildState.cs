using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Map;

namespace YhProj.Game.State
{
    // obj_move_flag 스테이트도 있구납
    public class BuildState : State
    {
        private GameObject copyTarget;
        private GridObject copyGrid;
        private StageData curStageData;
        private IBuildable buildable;

        // data 를 받아서 사용해야 할 듯?

        public BuildState(StageData _stageData, IBuildable _buildable)
        {
            buildable = _buildable;
            curStageData = _stageData;
        }
        /// <summary>
        /// object를 복사하여 캐싱하여 grid를 표현할 초기 함수
        /// </summary>
        /// <param name="_baseObject">복사할 object </param>
        public override void Enter(GameObject _go)
        {
            // set start position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 생각해보니 소숫점말고 1단위로 이동하여야 함
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);

                if(copyTarget != null)
                {
                    if(IsBuild())
                    {

                    }
                    else
                    {

                    }
                }
                else
                {
                    copyTarget = GameUtil.InstantiateResource<GameObject>(_go);
                    copyGrid = copyTarget.GetComponent<GridObject>();

                    GridData gridData = copyTarget.GetComponent<IGrid>().GridData;
                    copyGrid.Create(gridData);

                    int x = Mathf.FloorToInt(hit.point.x / curStageData.tileSize);
                    int z = Mathf.FloorToInt(hit.point.z / curStageData.tileSize);

                    copyTarget.transform.position = new Vector3(x, 0, z);
                }
            }
        }

        public override void Exit()
        {
            copyTarget = null;
            // 파란색 그리드 영역이라면 옮기거나 빌드를 해야 함
            base.Exit();
        }
        /// <summary>
        /// 복사한 오브젝트를 기준으로 gird를 생성하고 이동시키는 함수
        /// </summary>
        /// <param name="_baseObject"> grid를 표현할 오브젝트 건물</param>
        public override void Update()
        {
            // set move position
            if(copyTarget == null)
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 생각해보니 소숫점말고 1단위로 이동하여야 함
            if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);

                int x = Mathf.FloorToInt(hit.point.x / curStageData.tileSize);
                int z = Mathf.FloorToInt(hit.point.z / curStageData.tileSize);

                Vector3 move = new Vector3(x, 0, z);

                copyTarget.transform.position = move;
                IsBuild();
            }
        }

        private bool IsBuild()
        {
            bool ret = false;

            if(copyGrid != null)
            {
                ret = copyGrid.IsCheckGrid(buildable);
            }

            return ret;
        }
    }

}