/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using PD.Logger;
using TMPro;
using UnityEngine;

/// <summary>
///     Controls the Console Panel UI
/// </summary>
public sealed class DevConsolePanelUI : MonoBehaviour
{
    [SerializeField]
    private GameObject logPrefab;
    [SerializeField]
    private Transform logsContent;

    public void OnLogMessageReceived(string msg, string stackTrace, LogType type)
    {
        if (GameManager.IsDevMode)
        {
            GameObject logGameObject = Instantiate(logPrefab, logsContent);
            logGameObject.GetComponentInChildren<TextMeshProUGUI>().text = msg;
        }
    }
}
