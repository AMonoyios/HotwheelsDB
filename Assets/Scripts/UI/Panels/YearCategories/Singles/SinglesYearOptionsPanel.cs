/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HWAPI;
using SW.Logger;
using SW.Utils;
using TMPro;
using SW.Utils.ResourcesHandler;
using System.Linq;

public sealed class SinglesYearOptionsPanel : MultiPageNavigation<YearCategoriesModel>
{
    [Header("Extra navigation feature")]
    [SerializeField]
    private ScrollRect scrollRect;

    [Header("Page specific data")]
    [SerializeField]
    private Transform container;
    [SerializeField]
    private GameObject yearOptionPrefab;

    private const string undesiredOption = "5-Packs";

    public override void OnComplete()
    {
        scrollRect.verticalNormalizedPosition = 1;
    }

    public override void PageBehaviour(YearCategoriesModel data)
    {
        for (int i = 0; i < data.YearCategories.Count; i++)
        {
            string yearTitle = data.YearCategories[i].title;

            data.YearCategories[i].title = yearTitle.Replace(" ", "_");
            data.YearCategories[i].label = yearTitle.Replace("Category:", "");
            data.YearCategories[i].targetPageTitle  = $"List_of_{CoreResources.GetIntFromString(yearTitle)}_Hot_Wheels";
        }

        data.YearCategories = data.YearCategories.OrderBy(entry => entry.title).ToList();
        data.YearCategories.Reverse();

        container.DestroyAllChildren();

        for (int i = 0; i < data.YearCategories.Count; i++)
        {
            // HACK: This is a cheaty way to skip the 5-Packs in the category of singles.
            if (data.YearCategories[i].title.Contains(undesiredOption))
            {
                CoreLogger.LogWarning($"Skipped {data.YearCategories[i].label}. Does not match current request requirements");
                continue;
            }

            Instantiate(yearOptionPrefab, container)
                .GetComponent<SinglesYearOption>()
                .Init(data.YearCategories[i].label, data.YearCategories[i].title, data.YearCategories[i].targetPageTitle);
        }
    }
}
