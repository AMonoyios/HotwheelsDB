/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using SW.UI;
using UnityEngine;

public sealed class CloseButton : BaseButton
{
    public override void Behaviour()
    {
        PanelManager.Instance.HideLastPanel();
    }
}
