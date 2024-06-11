using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Map
{
    [System.Serializable]
    public class GridData : GameData
    {
        public int sizeX;
        public int sizeY;
        public int sizeZ;
        public int gridSize;
    }

    public interface IGrid
    {
        public GridData GridData { get; set; }
    }
    // component로 사용할 것임,, 여기서 gridobjects 셋팅을 다 해놔야겠다;;
    public class GridObject : MonoBehaviour
    {
        // 해당 건물의 제일 왼쪽 하단 오브젝트 피봇을 위해 생성
        [SerializeField]
        private Transform gridRoot;

        [SerializeField]
        private List<GameObject> gridObjList = new List<GameObject>();
        // 각 오브젝트에 붙어질 그리드 메테리얼 리스트
        [SerializeField]
        private List<Material> matList = new List<Material>();
        [SerializeField]
        private Color enableColor;
        [SerializeField]
        private Color disableColor;

        public GridData gridData;
        // 이 부분 objectlist 껏다 켰다 하는 부분 만들어야 함
        public void Create(GridData _data)
        {
            gridData = _data;

            Vector3 gridPos = new Vector3(gridData.sizeX / 2, 0, gridData.sizeY / 2);

            for (int i = 0; i < gridData.sizeX; i++)
            {
                float x = gridData.gridSize / 2 * i;

                for(int j = 0; j < gridData.sizeZ; j++)
                {
                    float z = gridData.gridSize / 2 * j;

                    Vector3 sumPos = gridPos + new Vector3(x, 0, z);

                    int idx = i * gridData.sizeX + j;

                    // 만약 없다면 생성을 해야 함
                    gridObjList[idx].transform.parent = gridRoot;
                    gridObjList[idx].transform.localPosition = sumPos;
                }
            }

            gridRoot.gameObject.SetActive(true);
        }
        public void GridOnOff(Material _mat, bool _isValue)
        {
            if(_isValue)
            {
                _mat.SetColor("_TintColor", enableColor);
            }
            else
            {
                _mat.SetColor("_TintColor", disableColor);
            }
        }
        public void Delete()
        {
            gridRoot.gameObject.SetActive(false);
        }

        public bool IsCheckGrid(IBuildable _buildable)
        {
            bool ret = false;

            if(gridData == null) 
            {
                return ret;
            }

            // float이 될 수가 없지;;
            // 어떻게든 정수로 만들어서 사용하여야 함!!
            // 규칙을 정하여야 함 그리드는 피봇을 다르게 잡아야 할지도?

            for (int i = 0; i < gridObjList.Count; i++)
            {
                // 오브젝트 안에 있는 로컬 포지션을 월드 포지션으로 변경하여 검사를 시작한다
                Vector3 gridPos = gridObjList[i].transform.position;

                int x = Mathf.FloorToInt(gridPos.x);
                int z = Mathf.FloorToInt(gridPos.z);

                Vector3Int startGrid = new Vector3Int(x, 0, z);
                Vector2Int size = new Vector2Int(gridData.gridSize, gridData.gridSize);

                if (_buildable.IsBuildable(startGrid, size))
                {
                    GridOnOff(matList[i], true);
                }
                else
                {
                    GridOnOff(matList[i], false);
                    ret = false;
                }
            }

            return ret;
        }
    }
}

