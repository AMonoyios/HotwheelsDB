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

    private void Start()
    {
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.Log("DEV MODE");
            devMode = true;
        #endif

        devButton.gameObject.SetActive(devMode);
        devPanel.SetActive(false);
        CoreLogger.SetDevMode(devMode);

        Application.logMessageReceived += OnLogMessageReceived;
    }

    private void OnLogMessageReceived(string msg, string stackTrace, LogType type)
    {
        if (devMode)
        {
            GameObject logGameObject = Instantiate(devLogPrefab, devLogsContent);
            logGameObject.GetComponent<TextMeshProUGUI>().text = msg;
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
