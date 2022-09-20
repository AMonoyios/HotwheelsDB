/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;
using UnityEngine.UI;
using SW.UI;

[RequireComponent(typeof(Button))]
public sealed class ShowPanelBtn : MonoBehaviour
{
    [SerializeField]
    private Button showBtn;
    [SerializeField]
    private string panelId;

    // Start is called before the first frame update
    private void Start()
    {
        showBtn.onClick.AddListener(() => PanelManager.Instance.ShowPanel(panelId));
    }
}
