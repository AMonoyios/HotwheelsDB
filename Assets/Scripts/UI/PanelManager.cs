/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using SW.Logger;
using UnityEngine;

namespace SW.UI
{
    /// <summary>
    ///     The manager for all panels in the UI system.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public sealed class PanelManager : MonoPersistentSingleton<PanelManager>
    {
        [SerializeField]
        private Transform panelsParent;

        [SerializeField]
        private List<PanelModel> panels;

        private readonly List<PanelModelInstance> panelInstanceList = new();

        public void ShowPanel(string id)
        {
            PanelModel panelInstance = panels.Find(panel => panel.id == id);

            if (panelInstance == null)
            {
                // panel was not found in panelManager
                CoreLogger.LogError($"Panel with id: {id} was not found in the PanelManager.", gameObjectSource: gameObject);
            }
            else
            {
                // panel found
                CoreLogger.LogMessage($"Found panel with id: {id} in the PanelManager.", gameObjectSource: gameObject);

                // checking if panel already exists in the instance list
                if (panelInstanceList.Find(instance => instance.ID == id) != null)
                {
                    CoreLogger.LogMessage($"Instance of panel with id: {id} already exists.", gameObjectSource: panelsParent.gameObject);

                    // checking if the panel allows multiple instances of itself
                    if (!panelInstance.GetBasePanel.AllowMultipleInstances)
                    {
                        CoreLogger.LogWarning($"Panel with id: {id} does not allow multiple instances of itself.");
                        return;
                    }
                }

                // instantiate the new panel
                GameObject newPanelInstance = Instantiate(panelInstance.prefab, panelsParent);
                newPanelInstance.SetActive(true);

                // add the new panel to the queue of panels
                panelInstanceList.Add(new(id, newPanelInstance));
            }
        }

        public void HideLastPanel()
        {
            if(IsAnyPanelShowing())
            {
                var lastPanel = panelInstanceList[^1];
                panelInstanceList.Remove(lastPanel);

                Destroy(lastPanel.PrefabInstance);
            }
        }

        public bool IsAnyPanelShowing()
        {
            return GetPanelsInQueueCount() > 0;
        }

        public int GetPanelsInQueueCount()
        {
            return panelInstanceList.Count;
        }
    }
}
