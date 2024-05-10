using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game
{
    public static class CoroutineHandler
    {
        private static MonoBehaviour coroutineExecutor;

        private static void Initialize()
        {
            GameObject coroutineObject = new GameObject("CoroutineHandler");
            coroutineExecutor = coroutineObject.AddComponent<CoroutineExecutor>();
        }

        public static void StartCoroutine(IEnumerator coroutine)
        {
            if (coroutineExecutor == null)
            {
                Initialize();
            }

            coroutineExecutor.StartCoroutine(coroutine);
        }

        private class CoroutineExecutor : MonoBehaviour { }
    }
}
