/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SW.UI;

public sealed class ShowYearCategoriesForSinglesBtn : BaseButton
{
    public override void Behaviour()
    {
        PanelManager.Instance.ShowPanel("YearOptionsPanel");
    }
}
