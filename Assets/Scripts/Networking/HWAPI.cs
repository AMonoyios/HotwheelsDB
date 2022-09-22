/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using SW.Utils.Convertions;

namespace HWAPI
{
    public static class Queries
    {
        /// <summary>
        ///     Gets the first 100(by default, max is 500) years of hotwheels
        /// </summary>
        public static string YearCategory(ushort cmLimit = 100)
        {
            return $"https://hotwheels.fandom.com/api.php?action=query&list=categorymembers&cmlimit={cmLimit}&cmtitle=Category:Hot_Wheels_by_Year&format=json";
        }

        /// <summary>
        ///     Gets the data for specific year
        /// </summary>
        public static string YearOf(int targetYear, ushort cmLimit = 100)
        {
            return $"https://hotwheels.fandom.com/api.php?action=query&list=categorymembers&cmlimit={cmLimit}&cmtitle=Category:{targetYear}_Hot_Wheels&format=json";
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
                GameObject gameobject = new("NetworkingAPIRequests");
                Instance = gameobject.AddComponent<CoreNetworkingMonoBehaviour>();
            }
        }

        /// <summary>
        ///     Get the raw data from a url
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="onError">Action that will happen upon failed communication</param>
        /// <param name="onSuccess">Action that will happen upon successful communication</param>
        public static void Data(string url, Action<string> onError, Action<string> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(DataCoroutine(url, onError, onSuccess));
        }
        private static IEnumerator DataCoroutine(string url, Action<string> onError, Action<string> onSuccess)
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

        /// <summary>
        ///     Get the sprite from a url
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="onError">Action that will happen upon failed communication</param>
        /// <param name="onSuccess">Action that will happen upon successful communication</param>
        public static void Sprite(string url, Action<string> onError, Action<Sprite> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(SpriteCoroutine(url, onError, onSuccess));
        }
        private static IEnumerator SpriteCoroutine(string url, Action<string> onError, Action<Sprite> onSuccess)
        {
            using UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
                yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(www.error);
            }
            else
            {
                Texture2D result = (www.downloadHandler as DownloadHandlerTexture)?.texture;
                onSuccess(CoreConvertions.ConvertTexture2DToSprite(result));
            }
        }
    }
}
