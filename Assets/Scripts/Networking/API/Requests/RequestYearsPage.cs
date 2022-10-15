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
    public class RequestYearPage : FandomAPIRequest<YearCategoriesModel>
    {
        public static void GetYearPage(string cmcontinue, Action<string> onError, Action<YearCategoriesModel> onSuccess, int perPage = 10)
        {
            string url = $"https://hotwheels.fandom.com/api.php?action=query&list=categorymembers&cmlimit={perPage}&cmdir=descending&cmtitle=Category:Hot_Wheels_by_Year&format=json";

            if (Regex.Match(cmcontinue, "subcat\\|([^|]+)\\|\\w").Success)
            {
                Debug.Log($"*** Detected valid cmcontinue signature: {cmcontinue}");
                url += $"&cmcontinue={cmcontinue}";
            }

            Request
            (
                url: url,
                onError: (error) => onError(error),
                onSuccess: (yearCategoriesModel) => onSuccess(yearCategoriesModel)
            );
        }
    }
}
