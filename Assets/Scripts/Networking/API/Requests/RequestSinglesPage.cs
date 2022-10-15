/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SW.Logger;
using UnityEngine;
using UnityEngine.Networking;

namespace HWAPI
{
    public class RequestSinglesPage : FandomAPIRequest<SinglesPageModel>
    {
        public static void GetSinglesPage(string page, Action<string> onError, Action<SinglesPageModel> onSuccess)
        {
            string url = $"https://hotwheels.fandom.com/api.php?action=parse&page={page}&prop=sections&format=json";

            Request
            (
                url: url,
                onError: (error) => onError(error),
                onSuccess: (singlesPageModel) => onSuccess(singlesPageModel)
            );
        }
    }
}
