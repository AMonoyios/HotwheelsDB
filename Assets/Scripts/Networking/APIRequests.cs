/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace APIRequests
{
    public static class Info
    {
        /// <summary>
        ///     Gets the delta time between each connection attempt.
        /// </summary>
        public static float ConnectionRetryDelta
        {
            get
            {
                return 10.0f;
            }
        }
        public static int MaxConnectionRetry
        {
            get
            {
                return 2;
            }
        }
    }

    public static class Queries
    {
        public static string HotWheelsBy100FirstYears
        {
            get
            {
                return "https://hotwheels.fandom.com/api.php?action=query&list=categorymembers&cmlimit=100&cmtitle=Category:Hot_Wheels_by_Year&format=json";
            }
        }
        public static string HotWheelsYearCategory(string year)
        {
            return $"https://hotwheels.fandom.com/api.php?action=query&list=categorymembers&cmlimit=100&cmtitle={year}&format=json";
        }

        public static string HotWheelsByCollection
        {
            get
            {
                return "";
            }
        }
        public static string HotWheelsCollection(string collection)
        {
            return $"";
        }
    }

    public static class Request
    {
        private class CoreNetworkingMonoBehaviour : MonoPersistentSingleton<CoreNetworkingMonoBehaviour> {}
        private static CoreNetworkingMonoBehaviour Instance;
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject gameobject = new("NetworkingMB");
                Instance = gameobject.AddComponent<CoreNetworkingMonoBehaviour>();
            }
        }

        /// <summary>
        ///     Get the raw data from a url
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="onError">Action that will happen upon failed communication</param>
        /// <param name="onSuccess">Action that will happen upon successful communication</param>
        public static void RawData(string url, Action<string> onError, Action<string> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(FetchRawDataCoroutine(url, onError, onSuccess));
        }
        private static IEnumerator FetchRawDataCoroutine(string url, Action<string> onError, Action<string> onSuccess)
        {
            using UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
                yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(unityWebRequest.error);
            }
            else
            {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
        }
    }
}
