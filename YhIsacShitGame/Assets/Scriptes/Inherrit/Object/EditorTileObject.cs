using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Map
{
    public class EditorTileObject : TileObject
    {
        [Header("Color")]
        [SerializeField]
        private Color enemyRoadColor = Color.red;

        [SerializeField]
        private Color mineRoadColor = Color.white;

        [SerializeField]
        private Color decoRoadColor = Color.green;

        private LineRenderer lineRenderer;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();

            if (lineRenderer == null)
            {
                GameObject go = new GameObject("lineRenderer");
                lineRenderer = go.AddComponent<LineRenderer>();
            }

            lineRenderer.useWorldSpace = false;

            lineRenderer.transform.parent = transform;
            lineRenderer.transform.localPosition = new Vector3(-0.5f, 0, -0.5f);
            lineRenderer.startWidth = .15f;
            lineRenderer.endWidth = .15f;

            // top, bottom, left, right
            lineRenderer.positionCount = 6;
            lineRenderer.SetPosition(0, new Vector3(0, 0, 1));
            lineRenderer.SetPosition(1, new Vector3(1, 0, 1));
            lineRenderer.SetPosition(2, new Vector3(1, 0, 0));
            lineRenderer.SetPosition(3, new Vector3(0, 0, 0));
            lineRenderer.SetPosition(4, new Vector3(0, 0, 1));
            // 다시 돌아가는거 생각해야 함
            lineRenderer.SetPosition(5, new Vector3(1, 0, 1));
        }

        #region On/Off
        public override void Create<T>(T _baseData)
        {
            base.Create(_baseData);

            transform.localScale = Vector3.one * 0.8f;

            switch (tileData.elementType)
            {
                case ElementType.MINE:
                    meshRenderer.material.color = mineRoadColor;
                    break;
                case ElementType.ENEMY:
                    meshRenderer.material.color = enemyRoadColor;
                    break;
                case ElementType.DECO:
                    meshRenderer.material.color = decoRoadColor;
                    break;
            }
        }
        #endregion
    }
}
