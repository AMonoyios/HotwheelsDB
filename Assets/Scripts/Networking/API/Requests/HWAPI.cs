/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

namespace HWAPI
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Networking;

    // TODO_HIGH: Remove partial classes and make the different request subclasses that derive from a main execute request task (ask daniel if stuck - common)
    public static partial class Request
    {
        private static class Queries
        {
            // OBSOLETE: Old Queries - remove later
            #region Old Queries
            /*

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
            ///     Gets an image
            /// </summary>
            public static string ImageURL(string photoName, int photoSize = 50)
            {
                return "";
            }

            /// <summary>
            ///     Query to retrieve all sections for a car's page
            /// </summary>
            public static string PageSections(string carName)
            {
                return $"https://hotwheels.fandom.com/api.php?format=json&action=parse&page={UnityWebRequest.EscapeURL(carName.Replace(" ", "_"))}&prop=sections";
            }

            /// <summary>
            ///     Query for versions table for a specific car
            /// </summary>
            public static string VersionsTable(string carName, int sectionIndex)
            {
                return $"https://hotwheels.fandom.com/api.php?format=json&action=parse&page={UnityWebRequest.EscapeURL(carName.Replace(" ", "_"))}&prop=wikitext&section={sectionIndex}";
            }

            */
            #endregion

            public static string LocalMissingImage()
            {
                return "MissingImage.png";
            }

            public static string CloudMissingImage()
            {
                return "File:Image_Not_Available.jpg";
            }
        }

        #region MonoBehaviour
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
        #endregion

        /// <summary>
        ///     Get the raw data from a url
        /// </summary>
        public static void GetJsonData(string url, Action<string> onError, Action<string> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(GetJsonDataCoroutine(url, onError, onSuccess));
        }
        private static IEnumerator GetJsonDataCoroutine(string url, Action<string> onError, Action<string> onSuccess)
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
