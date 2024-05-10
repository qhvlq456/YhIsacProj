using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace YhProj.Game.Http
{
    public class HttpHandler
    {
        // 여기서 우선순위 조건 매기는 함수 필요할거 같은데..?
        public void Post(string url, string jsonData, Action<bool, string> callback)
        {
            CoroutineHandler.StartCoroutine(SendPostRequest(url, jsonData, callback));
        }

        private IEnumerator SendPostRequest(string url, string jsonData, Action<bool, string> callback)
        {
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke(true, request.downloadHandler.text);
            }
            else
            {
                callback?.Invoke(false, request.error);
            }
        }

    }
}

