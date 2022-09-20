/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;
using UnityEngine.UI;
using SW.UI;

[RequireComponent(typeof(Button))]
public sealed class HidePanelBtn : MonoBehaviour
{
    [SerializeField]
    private Button closeBtn;

    // Start is called before the first frame update
    private void Start()
    {
        closeBtn.onClick.AddListener(() => PanelManager.Instance.HideLastPanel());
    }
}
