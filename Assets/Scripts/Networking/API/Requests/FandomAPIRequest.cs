/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SW.Logger;
using UnityEngine;
using UnityEngine.Networking;

namespace HWAPI
{
    public class RequestAPIFandom : MonoPersistentSingleton<RequestAPIFandom>{}

    public class FandomAPIRequest<T>
    {
        public static void Request(string url, Action<string> onError, Action<T> onSuccess)
        {
            RequestAPIFandom.Instance.StartCoroutine(RequestCoroutine(url, onError, onSuccess));
        }
        private static IEnumerator RequestCoroutine(string url, Action<string> onError, Action<T> onSuccess)
        {
            using UnityWebRequest www = UnityWebRequest.Get(url);
                yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                onError($"Failed to fetch url {url}. Error: {www.error}");
            }
            else
            {
                string json = Encoding.UTF8.GetString(www.downloadHandler.data);
                T model = JsonConvert.DeserializeObject<T>(json);
                onSuccess(model);
            }
        }

        public static void BundleRequest(List<string> urls, Action<string> onError, Action<List<T>> onSuccess)
        {
            RequestAPIFandom.Instance.StartCoroutine(BundleRequestCoroutine(urls, onError, onSuccess));
        }
        private static IEnumerator BundleRequestCoroutine(List<string> urls, Action<string> onError, Action<List<T>> onSuccess)
        {
            int urlsCount = urls.Count;
            List<UnityWebRequestAsyncOperation> requests = new(urlsCount);

            for (int i = 0; i < urlsCount; i++)
            {
                using UnityWebRequest www = UnityWebRequest.Get(urls[i]);
                {
                    CoreLogger.LogMessage($"Adding request {www.url} to list", true);
                    requests.Add(www.SendWebRequest());
                }
            }

            CoreLogger.LogMessage("Waiting for requests to be fetched...", true);
            yield return new WaitUntil(() => AllRequestsDone(requests));
            CoreLogger.LogMessage("All requests collected.", true);

            List<T> responses = new();
            HandleAllRequestsWhenDone
            (
                requests: requests,
                onError: (error) => onError(error),
                onSuccess: (data) => responses.Add(data)
            );

            // foreach (UnityWebRequestAsyncOperation request in requests)
            // {
            //     request.webRequest.Dispose();
            // }

            Debug.Log("---------- Calling on success for bundle request");
            onSuccess(responses);
        }
        private static bool AllRequestsDone(List<UnityWebRequestAsyncOperation> requests)
        {
            return requests.All(r => r.isDone);
        }
        private static void HandleAllRequestsWhenDone(List<UnityWebRequestAsyncOperation> requests, Action<string> onError, Action<T> onSuccess)
        {
            for (int i = 0; i < requests.Count; i++)
            {
                UnityWebRequest www = requests[i].webRequest;

                // FIXME: ArgumentNullException: Value cannot be null. Parameter name: _unity_self
                if (www.result != UnityWebRequest.Result.Success)
                {
                    onError($"Failed to fetch url {requests[i].webRequest.url}. Error: {www.error}");
                }
                else
                {
                    string json = Encoding.UTF8.GetString(www.downloadHandler.data);
                    T model = JsonConvert.DeserializeObject<T>(json);
                    onSuccess(model);
                }
            }
        }

        // OBSOLETE: Old way to request data from API - remove
        #region Old Methods
        /*

        /// <summary>
        ///     WIP - Get the texture from a url
        /// </summary>
        public static void GetSprite(string url, Action<Sprite> onError, Action<Sprite> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(GetSpriteCoroutine(url, onError, onSuccess));
        }
        public static void GetSprite(string url, int overrideSize, Action<Sprite> onError, Action<Sprite> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(GetSpriteCoroutine(url, onError, onSuccess, overrideSize));
        }
        private static IEnumerator GetSpriteCoroutine(string url, Action<Sprite> onError, Action<Sprite> onSuccess, int overrideSize = -1)
        {
            CoreLogger.LogMessage($"Downloading image from URL: {url}");

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
                yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                CoreLogger.LogError($"{www.error}. Couldn't download image, Requesting local {Queries.LocalMissingImage()}");
                onError(Resources.Load<Sprite>(Queries.LocalMissingImage()));
            }
            else
            {
                string jsonRequestData = Encoding.UTF8.GetString(www.downloadHandler.data);
                ImageModel imageModel = JsonConvert.DeserializeObject<ImageModel>(jsonRequestData);

                if (imageModel.ImagesData[0] == null)
                {
                    imageModel.ImagesData[0] = new()
                    {
                        thumbnail = new()
                        {
                            source = Queries.ImageURL(Queries.CloudMissingImage())
                        }
                    };
                }

                string imageUrl = imageModel.ImagesData[0].thumbnail.source;

                // if (overrideSize > 0)
                //     imageUrl = imageUrl.Replace("75", overrideSize.ToString());

                www = UnityWebRequestTexture.GetTexture(imageUrl);
                    yield return www.SendWebRequest();

                Sprite sprite;
                if (www.result != UnityWebRequest.Result.Success)
                {
                    CoreLogger.LogError($"Request texture from url {imageUrl} failed! Returning local MissingImage.png.\n" +
                                        $"{www.error}");

                    sprite = Resources.Load<Sprite>(Queries.LocalMissingImage());
                }
                else
                {
                    Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    sprite = SW.Utils.Convertions.CoreConvertions.ConvertTexture2DToSprite(texture);

                    if (sprite == null)
                        CoreLogger.LogWarning($"Sprite for {imageUrl} failed to parse, returning null.");
                }

                onSuccess(sprite);
            }
        }

        /// <summary>
        ///     Returns a list of page sections for a specific car
        /// </summary>
        public static void GetCarPageSections(string carName, Action<string> onError, Action<List<PageSectionsModel.PageSection>> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(GetCarPageSectionsCoroutine(carName, onError, onSuccess));
        }
        private static IEnumerator GetCarPageSectionsCoroutine(string carName, Action<string> onError, Action<List<PageSectionsModel.PageSection>> onSuccess)
        {
            using UnityWebRequest www = UnityWebRequest.Get(Queries.PageSections(carName));
                yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(www.error);
            }
            else
            {
                string jsonRequestData = Encoding.UTF8.GetString(www.downloadHandler.data);
                PageSectionsModel pageSections = JsonConvert.DeserializeObject<PageSectionsModel>(jsonRequestData);

                List<PageSectionsModel.PageSection> sections = new();

                List<PageSectionsModel.Sections> sectionsList = pageSections.parse.sections;
                for (int sectionIndex = 0; sectionIndex < sectionsList.Count; sectionIndex++)
                {
                    PageSectionsModel.PageSection newSection = new
                    (
                        name: sectionsList[sectionIndex].name,
                        index: sectionsList[sectionIndex].index
                    );

                    sections.Add(newSection);
                }

                onSuccess(sections);
            }
        }

        /// <summary>
        ///     Gets the versions table for a car
        /// </summary>
        public static void GetCarVersionsTable(string carName, int sectionIndex, Action<string> onError, Action<List<VersionsTableCarInfo>> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(GetCarVersionsTableCoroutine(carName, sectionIndex, onError, onSuccess));
        }
        private static IEnumerator GetCarVersionsTableCoroutine(string carName, int sectionIndex, Action<string> onError, Action<List<VersionsTableCarInfo>> onSuccess)
        {
            using UnityWebRequest www = UnityWebRequest.Get(Queries.VersionsTable(carName, sectionIndex));
                yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(www.error);
            }
            else
            {
                string versionsTableJson = Encoding.UTF8.GetString(www.downloadHandler.data);
                VersionsTableModel versionsTable = JsonConvert.DeserializeObject<VersionsTableModel>(versionsTableJson);
                List<VersionsTableCarInfo> carInfoList = Parser.FromWikitext(versionsTable.Content());

                foreach (var carInfo in carInfoList.Select((Data, Index) => new { Data, Index }))
                {
                    string carPhotoQuery = carInfo.Data.Photo;

                    bool canContinueRequest = false;
                    GetJsonData(carPhotoQuery,
                        onError: (error) =>
                        {
                            CoreLogger.LogError($"Failed to fetch json data for: {carPhotoQuery}. {error}");
                            canContinueRequest = true;
                        },
                        onSuccess: (json) =>
                        {
                            string carPhotoJson = json;
                            Debug.Log("dame: " + carPhotoJson);

                            ImageModel imageModel = JsonConvert.DeserializeObject<ImageModel>(carPhotoJson);
                            Debug.Log($"XXX carinfoindex {carInfo.Index}, imageModelDataLength {imageModel.ImagesData.Length}");
                            ImageModel.Pages pages = imageModel.ImagesData[carInfo.Index];
                            Debug.Log("ns: " + pages.ns);
                            Debug.Log("id: " + pages.pageid);
                            Debug.Log("title: " + pages.title);
                            Debug.Log("source: " + pages.thumbnail);
                            Debug.Log("-----------------");

                            canContinueRequest = true;

                            string sourceImage = pages.thumbnail.source;
                            carInfoList[carInfo.Index].Photo = sourceImage;
                        }
                    );
                    while (!canContinueRequest)
                        yield return null;
                }

                onSuccess(carInfoList);
            }
        }
    }

    public static class Utils
    {
        /// <summary>
        ///     Returns the index of a section
        /// </summary>
        public static int GetIndexOfSection(string section, List<PageSectionsModel.PageSection> pageSections)
        {
            for (int i = 0; i < pageSections.Count; i++)
            {
                if (pageSections[i].Name == section)
                {
                    return pageSections[i].Index;
                }
            }
            return -1;
        }
        */
        #endregion
    }
}
