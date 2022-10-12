using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SW.UI
{
    public class HidePanelButton : BaseButton
    {
        public override void Behaviour()
        {
            // Hide the last panel
            _panelManager.HideLastPanel();
        }
    }
}
