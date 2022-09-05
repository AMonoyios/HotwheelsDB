/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using PD.Logger;

public class DeveloperController : MonoBehaviour
{
    [SerializeField]
    private Button devButton;
    [SerializeField]
    private GameObject devPanel;
    [SerializeField]
    private Transform devLogsContent;
    [SerializeField]
    private GameObject devLogPrefab;

    private bool devMode = false;
    public bool GetAppMode { get { return devMode; }}

    private void Start()
    {
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
            devMode = true;
        #endif

        devButton.gameObject.SetActive(devMode);
        devPanel.SetActive(false);
        CoreLogger.SetDevMode(devMode);

        Application.logMessageReceived += OnLogMessageReceived;

        if (devMode)
            CoreLogger.LogMessage("Game mode: DEV MODE");
        else
            CoreLogger.LogMessage("Game mode: Normal MODE");

    }

    private void OnLogMessageReceived(string msg, string stackTrace, LogType type)
    {
        if (devMode)
        {
            GameObject logGameObject = Instantiate(devLogPrefab, devLogsContent);
            logGameObject. <TextMeshProUGUI>().text = msg;
        }
    }

    public void EnableDevPanel()
    {
        devPanel.SetActive(true);
    }

    public void DisableDevPanel()
    {
        devPanel.SetActive(false);
    }
}
