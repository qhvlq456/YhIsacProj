using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YhProj.Game.Play
{
    // 계정 연동 밑 계정 생성
    public class Intro : MonoBehaviour, IGameStep
    {
        public event Action OnComplete = delegate { };

        private void Start()
        {
            Debug.Log("인트로를 시작합니다.");
        }

        public void StartStep()
        {
            
        }

        public void StopStep()
        {
            // 인트로가 끝나면 이벤트를 발생시킵니다.
            OnComplete?.Invoke();
        }
    }
}

