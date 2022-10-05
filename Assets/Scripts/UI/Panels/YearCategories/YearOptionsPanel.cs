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

public sealed class YearOptionsPanel : MultiPageNavigation<YearCategoriesModel> // TODO: The parent class that derived p to subclass to basic navigation model ena mbeni gia to ka8e panel
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
        // ---------- This is where you show all the data for each page -------------- //

        container.DestroyAllChildren();

        for (int i = 0; i < data.YearCategories.Count; i++)
        {
            YearOption newYearOption = Instantiate(yearOptionPrefab, container).GetComponent<YearOption>();
            newYearOption.yearLabel.text = data.YearCategories[i].label;
        }

        // --------------------------------------------------------------------------- //
    }
}
