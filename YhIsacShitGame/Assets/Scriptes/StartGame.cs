using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Play
{
    // 게임의 각 단계를 나타내는 기본 클래스입니다.
    public interface IGameStep
    {
        public void StartStep();
        public void StopStep();
    }

    public class StartGame : MonoBehaviour
    {
        private Intro intro;
        private Loading loading;
        private PlayGame playGame;

        public void Start()
        {
            // 각 단계를 초기화합니다.
            intro = new Intro();
            loading = new Loading();
            playGame = new PlayGame();

            // 각 단계의 완료 이벤트에 다음 단계를 실행하는 메서드를 연결합니다.
            intro.OnComplete += StartLoading;
            loading.OnComplete += StartPlayGame;
            playGame.OnComplete += FinishGame;

            // 인트로 단계를 시작합니다.
            intro.StartStep();
        }

        private void StartLoading()
        {
            // 로딩 단계를 시작합니다.
            loading.StartStep();
        }

        private void StartPlayGame()
        {
            // 게임 플레이 단계를 시작합니다.
            playGame.StartStep();
        }

        private void FinishGame()
        {
            Debug.Log("게임이 완료되었습니다.");
        }
    }
}
