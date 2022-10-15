/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SW.Logger;
using UnityEngine;
using UnityEngine.Networking;

namespace HWAPI
{
    public class RequestSinglesPageSection : FandomAPIRequest<SinglesPageSectionModel.Parse>
    {
        public static void GetSinglesPageSection(string page, int section, Action<string> onError, Action<SinglesPageSectionModel.Parse> onSuccess)
        {
            string url = $"https://hotwheels.fandom.com/api.php?action=parse&page={page}&section={section}&format=json";

            CoreLogger.LogMessage($"URL request: {url}", true);

            Request
            (
                url: url,
                onError: (error) => onError(error),
                onSuccess: (model) => onSuccess(model)
            );
        }

        public static void GetSinglesPageSection(string page, SinglesPageModel singlesPageModel, Action<string> onError, Action<List<SinglesPageSectionModel.Parse>> onSuccess)
        {
            List<string> urls = new();
            for (int sectionIndex = 1; sectionIndex <= singlesPageModel.PageSections.Count; sectionIndex++)
            {
                string url = $"https://hotwheels.fandom.com/api.php?action=parse&page={page}&section={sectionIndex}&format=json";

                CoreLogger.LogMessage($"URL {sectionIndex}: {url}", true);

                urls.Add(url);
            }

            BundleRequest
            (
                urls: urls,
                onError: (error) => onError(error),
                onSuccess: (parse) =>
                {
                    List<SinglesPageSectionModel.Parse> models = new();
                    for (int modelIndex = 0; modelIndex < parse.Count; modelIndex++)
                    {
                        List<SinglesPageSectionModel.Parse> sectionModels = Parser<SinglesPageSectionModel.Parse>.FromWikiText(parse[modelIndex].table.content);

                        for (int sectionIndex = 0; sectionIndex < sectionModels.Count; sectionIndex++)
                        {
                            CoreLogger.LogMessage($"Adding model {sectionModels[sectionIndex].title} to general models list.", true);

                            models.Add(sectionModels[sectionIndex]);
                        }
                    }

                    onSuccess(models);
                }
            );
        }
    }
}
