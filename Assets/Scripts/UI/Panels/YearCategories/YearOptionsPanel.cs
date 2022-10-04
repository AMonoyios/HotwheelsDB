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

public sealed class YearOptionsPanel : MultiPageNavigation
{
    [Header("Page specific data")]
    [SerializeField]
    private Transform container;
    [SerializeField]
    private GameObject yearOptionPrefab;

    public override void PageBehaviour()
    {
        // ---------- This is where you show all the data for each page -------------- //

        container.DestroyAllChildren();

        // TODO: mql5

        for (int i = 0; i < yearPages.YearCategories.Count; i++)
        {
            YearOption newYearOption = Instantiate(yearOptionPrefab, container).GetComponent<YearOption>();
            newYearOption.yearLabel.text = yearPages.YearCategories[i].label;
        }

        // --------------------------------------------------------------------------- //
    }
}
