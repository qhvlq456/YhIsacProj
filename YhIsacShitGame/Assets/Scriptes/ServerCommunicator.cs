using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YhProj.Game.Http;

namespace YhProj.Game
{
    public static class ServerCommunicator
    {
        private static readonly string serverUrl = "http://example.com/api/data"; // 서버의 API 엔드포인트 URL
        private static HttpHandler httpHandler;


        public static event EventHandler<ServerCallbackEventArgs> OnServerCallback;

        // 의존성 주입을 위한 메서드
        public static void Initialize(HttpHandler handler)
        {
            httpHandler = handler;
        }

        public static void SendData(params ServerReceiveEventArgs[] _data)
        {
            string jsonData = JsonUtility.ToJson(_data); // 게임 데이터를 JSON 문자열로 변환

            httpHandler.Post(serverUrl, jsonData, (_success, _response) =>
            {
                if (_success)
                {
                    Debug.Log("Data sent to server successfully.");
                    // 서버 응답 데이터 처리 (OnServerCallback 이벤트 트리거 포함)
                    OnServerCallbackReceived(null, new ServerCallbackEventArgs(_success, _response));
                }
                else
                {
                    Debug.LogError($"Failed to send data to server: {_response}");
                    // 오류 처리
                }
            });
        }

        public static void SendData<T>(params T[] dataList) where T : GameData
        {
            // 여러 데이터를 하나의 JSON 배열로 변환하는 로직 추가 필요
            // ...

            // 변환된 JSON 배열을 서버로 전송 (SendData<T>(T data) 메서드 활용)
            // ...
        }

        private static void OnServerCallbackReceived(object sender, ServerCallbackEventArgs e)
        {
            // 이벤트 처리 메서드 목록 가져오기
            List<EventHandler<ServerCallbackEventArgs>> eventHandlers = OnServerCallback?.GetInvocationList().Select(d => (EventHandler<ServerCallbackEventArgs>)d).ToList();

            // 이벤트 처리 메서드 실행
            foreach (EventHandler<ServerCallbackEventArgs> handler in eventHandlers)
            {
                // 기존 OnServerCallbackReceived 함수 내용 유지
                if (e.success)
                {
                    Debug.Log("Data sent to server successfully.");
                    handler?.Invoke(sender, e);
                }
                else
                {
                    Debug.LogError($"Failed to send data to server: {e.response}");
                }
            }
        }
    }
    public class ServerCallbackEventArgs : EventArgs
    {
        public bool success { get; private set; }
        public string response { get; private set; }

        public ServerCallbackEventArgs(bool _success, string _response)
        {
            success = _success;
            response = _response;
        }
    }
    // 후에 수정
    public class ServerReceiveEventArgs : EventArgs
    {
        public bool success { get; private set; }
        public string response { get; private set; }

        public ServerReceiveEventArgs(bool _success, string _response)
        {
            success = _success;
            response = _response;
        }
    }
}
