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

/// <summary>
///     Controls the General Panel UI
/// </summary>
public sealed class DevGeneralPanelUI : MonoBehaviour
{
    [Header("User/App info")]
    [SerializeField]
    private TextMeshProUGUI versionTxt;
    [SerializeField]
    private TextMeshProUGUI userIDTxt;

    [Header("Time info")]
    [SerializeField]
    private TextMeshProUGUI localTimeTxt;
    private string localTimeCached;
    [SerializeField]
    private TextMeshProUGUI lastServerUpdateTimeTxt;

    [Header("Buttons")]
    [SerializeField]
    private Button restartButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => RestartApp());
        InitGeneralInfo();
    }

    private void InitGeneralInfo()
    {
        versionTxt.text += Application.version;
        userIDTxt.text += SystemInfo.deviceName;
        localTimeCached = localTimeTxt.text;
        localTimeTxt.text += GameManager.GetLocalTime();
        //FIXME: when API is set replace this with the last update of the api page.
        lastServerUpdateTimeTxt.text += GameManager.GetLocalDate() + " | " + GameManager.GetLocalTime();
        CoreLogger.LogMessage("Initializing general info.");
    }

    private void OnEnable()
    {
        UpdateTimeInfo();
    }

    private void UpdateTimeInfo()
    {
        //FIXME: When API is set add the lastServerUpdateTimeTxt here.
        localTimeTxt.text = localTimeCached + GameManager.GetLocalDate() + " | " + GameManager.GetLocalTime();
        CoreLogger.LogMessage("Updating time info.");
    }

    public static void RestartApp()
    {
        GameManager.Restart();
    }
}
