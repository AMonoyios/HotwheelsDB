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

public sealed class YearOptionsPanel : MultiPageNavigation<YearCategoriesModel>
{
    [Header("Extra navigation feature")]
    [SerializeField]
    private ScrollRect scrollRect;

    [Header("Page specific data")]
    [SerializeField]
    private Transform container;
    [SerializeField]
    private GameObject yearOptionPrefab;

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
            data.YearCategories[i].page  = $"List_of_{CoreResources.GetIntFromString(yearTitle)}_Hot_Wheels";
        }

        data.YearCategories = data.YearCategories.OrderBy(entry => entry.title).ToList();
        data.YearCategories.Reverse();

        container.DestroyAllChildren();

        for (int i = 0; i < data.YearCategories.Count; i++)
        {
            Instantiate(yearOptionPrefab, container)
                .GetComponent<YearOption>()
                .Init(data.YearCategories[i].label, data.YearCategories[i].title, data.YearCategories[i].page);
        }
    }
}
