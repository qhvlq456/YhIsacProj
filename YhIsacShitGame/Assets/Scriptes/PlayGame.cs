using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YhProj.Game.Play
{
    /// <summary>
    /// intro -> loading -> game flow 클래스
    /// </summary>
    public class PlayGame : MonoBehaviour, IGameStep
    {
        public event Action OnComplete = delegate { };

        private void Start()
        {
            Debug.Log("게임을 시작합니다.");
        }

        public void StartStep()
        {
            Managers.Instance.LoadAllManagers();
        }
        // 게임이 끝나면 이벤트를 발생시킵니다.
        public void StopStep()
        {
            OnComplete?.Invoke();
        }
    }
}

