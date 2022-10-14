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
        public static void GetSinglesPageSection(string page, int section, Action<string> onError, Action<SinglesPageSectionModel> onSuccess)
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(GetSinglesPageSectionCoroutine(page, section, onError, onSuccess));
        }
        private static IEnumerator GetSinglesPageSectionCoroutine(string page, int section, Action<string> onError, Action<SinglesPageSectionModel> onSuccess)
        {
            string url = $"https://hotwheels.fandom.com/api.php?action=parse&page={page}&section={section}&format=json";

            // TODO_HIGH: Make this a base class that can be overwritten
            using UnityWebRequest www = UnityWebRequest.Get(url);
                yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                onError($"Failed to fetch url {url}. Error: {www.error}");
            }
            else
            {
                string json = Encoding.UTF8.GetString(www.downloadHandler.data);
                SinglesPageSectionModel model = JsonConvert.DeserializeObject<SinglesPageSectionModel>(json);

                onSuccess(model);
            }
        }
    }
}
