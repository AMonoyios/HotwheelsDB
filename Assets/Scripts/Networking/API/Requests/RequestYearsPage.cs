/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace HWAPI
{
    public static partial class Request
    {
        public static void GetYearPage<T>(string cmcontinue, Action<string> onError, Action<T> onSuccess) where T : BaseNavigateModel
        {
            if (Instance == null)
                Init();

            Instance.StartCoroutine(GetYearsCoroutine(cmcontinue, onError, onSuccess));
        }
        private static IEnumerator GetYearsCoroutine<T>(string cmcontinue, Action<string> onError, Action<T> onSuccess) where T : BaseNavigateModel
        {
            const uint perPage = 20;
            string url = $"https://hotwheels.fandom.com/api.php?action=query&list=categorymembers&cmend={perPage}&cmdir=descending&cmtitle=Category:Hot_Wheels_by_Year&format=json";

            if (Regex.Match(cmcontinue, "subcat\\|([^|]+)\\|\\w").Success)
            {
                Debug.Log($"*** Detected valid cmcontinue signature: {cmcontinue}");
                url += $"&cmcontinue={cmcontinue}";
            }

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
                T model = JsonConvert.DeserializeObject<T>(json);

                onSuccess(model);
            }
        }
    }
}
