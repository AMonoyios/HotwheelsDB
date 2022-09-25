/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Newtonsoft.Json;
using System.Text;
using SW.Logger;

namespace HWAPI
{
    public static class Request
    {
        private static class Queries
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
                return $"https://hotwheels.fandom.com/api.php?format=json&action=parse&page={carName}&prop=sections";
            }

            /// <summary>
            ///     Query for versions table for a specific car
            /// </summary>
            public static string VersionsTable(string carName, int sectionIndex)
            {
                return $"https://hotwheels.fandom.com/api.php?format=json&action=parse&page={carName}&prop=wikitext&section={sectionIndex}";
            }
        }

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

                if (imageModel.ImagesData[0] == null)
                {
                    imageModel.ImagesData[0] = new()
                    {
                        thumbnail = new()
                        {
                            source = Queries.ImageURL("File:Image_Not_Available.jpg")
                        }
                    };
                }

                // Debug.Log(imageModel.ImagesData[0].thumbnail.source);
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
                    CoreLogger.LogMessage($"Downloading {www.downloadHandler.data}");
                    texture2D = ((DownloadHandlerTexture)www.downloadHandler).texture;
                }

                CoreLogger.LogMessage($"executing onSuccess with {texture2D.name}");
                onSuccess(texture2D);
            }
        }

        /// <summary>
        ///     Returns a list of page sections for a specific car
        /// </summary>
        public static void CarPageSections(string carName, Action<string> onError, Action<List<PageSectionsModel.PageSection>> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(CarPageSectionsCoroutine(carName, onError, onSuccess));
        }
        private static IEnumerator CarPageSectionsCoroutine(string carName, Action<string> onError, Action<List<PageSectionsModel.PageSection>> onSuccess)
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
        public static void CarVersionsTable(string carName, int sectionIndex, Action<string> onError, Action<List<VersionsTableCarInfo>> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(CarVersionsTableCoroutine(carName, sectionIndex, onError, onSuccess));
        }
        private static IEnumerator CarVersionsTableCoroutine(string carName, int sectionIndex, Action<string> onError, Action<List<VersionsTableCarInfo>> onSuccess)
        {
            using UnityWebRequest www = UnityWebRequest.Get(Queries.VersionsTable(carName, sectionIndex));
                yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(www.error);
            }
            else
            {
                string versionsTableDataJson = Encoding.UTF8.GetString(www.downloadHandler.data);
                VersionsTableModel versionsTable = JsonConvert.DeserializeObject<VersionsTableModel>(versionsTableDataJson);
                List<VersionsTableCarInfo> versionsTableCarInfo = Parser.FromWikitext(versionsTable.Content());

                for (int versionsIndex = 0; versionsIndex < versionsTableCarInfo.Count; versionsIndex++)
                {
                    string versionsTableCarPhotoJson = versionsTableCarInfo[versionsIndex].Photo;

                    Debug.Log($"{versionsIndex}: {versionsTableCarInfo[versionsIndex].Photo}");

                    ImageModel imageModel = JsonConvert.DeserializeObject<ImageModel>(versionsTableCarPhotoJson);
                    versionsTableCarInfo[versionsIndex].Photo = imageModel.ImagesData[versionsIndex].thumbnail.source;
                }

                onSuccess(versionsTableCarInfo);
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
    }
}
