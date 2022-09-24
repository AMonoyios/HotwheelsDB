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
using Newtonsoft.Json;
using System.Text;
using SW.Logger;

namespace HWAPI
{
    public static class Queries
    {
        /// <summary>
        ///     Gets the first 100(by default, max is 500) years of hotwheels
        /// </summary>
        public static string AllYears(ushort cmLimit = 100)
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

        /// <summary>
        ///     Gets the versions for a specific car name
        /// </summary>
        /// <param name="carName">FIXME: the default value is a placeholder only for testing purposes. please remove later.</param>
        public static string CarVersionsTable(string carName = "BMW_M3_E46")
        {
            return $"https://hotwheels.fandom.com/api.php?format=json&action=parse&page={carName}&prop=wikitext&section=2";
        }

        /// <summary>
        ///     Gets an image
        /// </summary>
        /// <param name="photoName">Desired image/file name</param>
        /// <param name="photoSize">the size of the photo</param>
        public static string ImageURL(string photoName, int photoSize = 50)
        {
            return $"https://hotwheels.fandom.com/api.php?format=json&action=query&titles={photoName}&prop=pageimages&pithumbsize={photoSize}";
        }
    }

    public static class Request
    {
        private class CoreNetworkingMonoBehaviour : MonoPersistentSingleton<CoreNetworkingMonoBehaviour> {}
        private static CoreNetworkingMonoBehaviour Instance;
        private static void Init()
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
        ///     Get the texture from a url
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="onError">Action that will happen upon failed communication</param>
        /// <param name="onSuccess">Action that will happen upon successful communication</param>
        public static void Texture2D(string url, Action<Texture2D> onError, Action<Texture2D> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(Texture2DCoroutine(url, onError, onSuccess));
        }
        private static IEnumerator Texture2DCoroutine(string url, Action<Texture2D> onError, Action<Texture2D> onSuccess)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
                yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                CoreLogger.LogError($"{www.error}. Couldn't load image, Requesting NoImageFound sprite.");
                onError(Resources.Load<Texture2D>("MissingImage.png"));
            }
            else
            {
                string jsonRequestData = Encoding.UTF8.GetString(www.downloadHandler.data);
                ImageModel imageModel = JsonConvert.DeserializeObject<ImageModel>(jsonRequestData);

                Debug.Log(imageModel.ImagesData[0].thumbnail.source);
                string imageUrl = imageModel.ImagesData[0].thumbnail.source;

                www = UnityWebRequestTexture.GetTexture(imageUrl);
                    yield return www.SendWebRequest();

                Texture2D texture2D;
                if (www.result != UnityWebRequest.Result.Success)
                {
                    CoreLogger.LogError(www.error);
                    texture2D = Resources.Load<Texture2D>("MissingImage.png");
                }
                else
                {
                    texture2D = ((DownloadHandlerTexture)www.downloadHandler).texture;
                }

                onSuccess(texture2D);
            }
        }

        public static void CarVersionsTable(string url, Action<string> onError, Action<List<VersionsTableCarInfo>> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(CarVersionsTableCoroutine(url, onError, onSuccess));
        }
        private static IEnumerator CarVersionsTableCoroutine(string url, Action<string> onError, Action<List<VersionsTableCarInfo>> onSuccess)
        {
            using UnityWebRequest www = UnityWebRequest.Get(url);
                yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(www.error);
            }
            else
            {
                string jsonRequestData = Encoding.UTF8.GetString(www.downloadHandler.data);
                VersionsTableModel versionsTable = JsonConvert.DeserializeObject<VersionsTableModel>(jsonRequestData);
                onSuccess(Parser.FromWikitext(versionsTable.Content()));
            }
        }
    }
}
