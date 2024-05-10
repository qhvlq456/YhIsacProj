using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using YhProj.Game.Resource;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace YhProj.Game.Play
{
    // 번들 불러오기 밑 게임 씬 오브젝트들 생성
    public class Loading : MonoBehaviour, IGameStep
    {
        public event Action OnComplete = delegate { };
        public Slider loadingSlider; // 로딩 진행 상황을 표시할 슬라이더

        private void Start()
        {
            Debug.Log("로딩을 시작합니다.");
            // 로딩이 끝나면 이벤트를 발생시킵니다.
        }
        public void StartStep()
        {
            // 주어진 Addressable Asset 주소
            string assetAddress = "http://example.com/game_asset"; // 가상의 주소

            // Addressable Asset을 비동기적으로 로드하는 코루틴 시작
            StartCoroutine(LoadAddressableAsset(assetAddress));
        }

        public void StopStep()
        {
            OnComplete?.Invoke();
        }

        // 서버에서 받아서 할지 아님 로컬에서 받을지 생각을 해야 함
        private IEnumerator CoLoadBundleDataList()
        {
            // 나중에 cdn url로 변경 현제 로컬로 사용하자 .. 에셋번들리스트를 받아오는거였음 ㅎ
            string bundleUrl = "http://example.com/game_bundle"; // 가상의 번들 URL
            float totalProgress = 0f;

            // UnityWebRequest를 사용하여 번들을 다운로드합니다.
            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
            {
                // 번들 다운로드 시작
                yield return www.SendWebRequest();

                // 에러 체크
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load bundle from URL: {bundleUrl}, Error: {www.error}");
                    yield break;
                }

                if (www.downloadProgress > 0)
                {
                    loadingSlider.value = totalProgress / www.downloadProgress;
                }

                // cdn에서 번들 리스트 받아오기
                string json = www.downloadHandler.text;
            }
        }
        private IEnumerator LoadAddressableAsset(string assetAddress)
        {
            ResourceManager manager = Managers.Instance.GetManager<ResourceManager>();

            List<string> bundleNames = new List<string>();

            foreach (string bundleName in bundleNames)
            {
                // 에셋 번들에서 사용할 각 에셋을 비동기적으로 로드합니다.
                AsyncOperationHandle<UnityEngine.Object> assetHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(bundleName);
                yield return assetHandle;

                if (assetHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    // 로드된 에셋을 해당하는 형식으로 캐싱합니다.
                    // 이 부분에서 리소스 매니저에 캐싱하는 코드를 작성합니다.
                }
                else
                {
                    Debug.LogError($"Failed to load asset {bundleName} from AssetBundle {bundleName}: {assetHandle.OperationException}");
                }


                // 로드한 에셋 번들을 해제합니다.
                Addressables.Release(assetHandle);
            }
        }

    }
}
