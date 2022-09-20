/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;

namespace SW.UI
{
    /// <summary>
    ///     Model that hold the data for the queued panels.
    /// </summary>
    public sealed class PanelModelInstance
    {
        public PanelModelInstance(string id, GameObject prefabInstance)
        {
            this.ID = id;
            this.PrefabInstance = prefabInstance;
        }

        public string ID { get; }
        public GameObject PrefabInstance { get; }
    }
}
