using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Map;
using YhProj.Game.UI;

namespace YhProj.Game.State
{
    // 선택에 대한 행동을 구현해야 함
    public class SelectState : State
    {
        #region Factory Method
        public SelectState() { }
        public SelectState(BaseObject _baseObject) 
        { 
            
        }
        // 팩토리 메서드
        public static SelectState Create()
        {
            return new SelectState();
        }
        #endregion

        public override void Enter(BaseObject _baseObject)
        {
            EditorTileObject editorTileObject = _baseObject as EditorTileObject;
            // 초기화 설정
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

