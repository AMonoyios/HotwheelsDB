/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace HWAPI
{
    public static partial class Request
    {
        public static void GetSinglesPage(string page, Action<string> onError, Action<SinglesPageModel> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(GetSinglesPageCoroutine(page, onError, onSuccess));
        }
        private static IEnumerator GetSinglesPageCoroutine(string page, Action<string> onError, Action<SinglesPageModel> onSuccess)
        {
            string url = $"https://hotwheels.fandom.com/api.php?action=parse&page={page}&prop=sections&format=json";

            UnityWebRequest www = UnityWebRequest.Get(url);
                yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                onError($"Failed to fetch url {url}. Error: {www.error}");
            }
            else
            {
                string json = Encoding.UTF8.GetString(www.downloadHandler.data);
                SinglesPageModel model = JsonConvert.DeserializeObject<SinglesPageModel>(json);

                onSuccess(model);
            }
        }
    }
}
