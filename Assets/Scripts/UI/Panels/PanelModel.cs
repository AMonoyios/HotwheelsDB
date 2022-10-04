/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using SW.Logger;
using UnityEngine;

namespace SW.UI
{
    /// <summary>
    ///     The model for all panels in the UI system.
    /// </summary>
    [System.Serializable]
    public sealed class PanelModel
    {
        public string id;
        public GameObject prefab;

        private BasePanel _baseModel;
        public BasePanel GetBasePanel
        {
            get
            {
                if (_baseModel == null)
                {
                    _baseModel = prefab.GetComponent<BasePanel>();
                    CoreLogger.LogMessage($"Fetching BasePanel component from panel with id: {id}.");
                }

                return _baseModel;
            }
        }
    }
}
