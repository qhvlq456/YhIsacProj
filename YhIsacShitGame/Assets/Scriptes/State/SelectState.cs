namespace YhProj
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // ���ÿ� ���� �ൿ�� �����ؾ� ��
    public class SelectState : State
    {
        #region Factory Method
        public SelectState() { }
        public SelectState(BaseObject _baseObject) 
        { 
            
        }
        // ���丮 �޼���
        public static SelectState Create()
        {
            return new SelectState();
        }
        #endregion

        public override void Enter(BaseObject _baseObject)
        {
            EditorTileObject editorTileObject = _baseObject as EditorTileObject;
            // �ʱ�ȭ ����
            if (editorTileObject != null)
            {
                editorTileObject.Load(editorTileObject.tileData);
                Managers.Instance.GetManager<UIManager>().ShowUI<MapToolUI, EditorTileObject>("MapToolUI", editorTileObject);
            }
            else
            {
                Debug.LogError($"target is null");
            }
        }
        public override void Update()
        {
            
        }

        public override void Exit()
        {
            Debug.Log("SelectState click exit!!");
        }

    }
}

