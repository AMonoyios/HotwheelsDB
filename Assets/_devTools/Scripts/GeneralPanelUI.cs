/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
///     Controls the General Panel UI
/// </summary>
public sealed class GeneralPanelUI : MonoBehaviour
{
    [Header("User/App info")]
    [SerializeField]
    private TextMeshProUGUI versionTxt;
    [SerializeField]
    private TextMeshProUGUI userIDTxt;

    [Header("Time info")]
    [SerializeField]
    private TextMeshProUGUI localTimeTxt;
    [SerializeField]
    private TextMeshProUGUI lastServerUpdateTimeTxt;

    [Header("Buttons")]
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button testNotificationButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => RestartApp());
    }

    public static void RestartApp()
    {
        GameManager.Restart();
    }
}
