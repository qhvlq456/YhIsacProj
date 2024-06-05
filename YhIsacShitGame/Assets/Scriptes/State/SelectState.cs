using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj.Game.Map;
using YhProj.Game.UI;

namespace YhProj.Game.State
{
    // 선택에 대한 행동을 구현해야 함
    // 오브젝트에 대한 클릭인가? build를 하기위한클릭인가?, drag를 위한 클릭인가?
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
                editorTileObject.Create(editorTileObject.gameData);
                // 후에 ui paramter같은 클래스를 만들어서 사용
                // Managers.Instance.GetManager<UIManager>().ShowUI<MapToolBodyUI, EditorTileObject>(UIRootType.Contextual,"MapToolUI", editorTileObject);
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

