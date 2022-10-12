/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShowYearCategoriesForSinglesBtn : BaseButton
{
    [Header("Panel specific properties")]
    /// <summary>
    ///     The id of the panel to show
    /// </summary>
    [SerializeField]
    private string panelId;

    /// <summary>
    ///     The panel show behaviour
    /// </summary>
    [SerializeField]
    private PanelShowBehaviour behaviour;

    public override void Behaviour()
    {
        // Show the panel
        _panelManager.ShowPanel(panelId, behaviour);
    }
}
