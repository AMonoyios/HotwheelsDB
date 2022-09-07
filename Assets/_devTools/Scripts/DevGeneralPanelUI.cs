/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PD.Android;

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
    [SerializeField]
    private TextMeshProUGUI lastServerUpdateTimeTxt;

    [Header("Buttons")]
    [SerializeField]
    private Button restartButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => RestartApp());
    }

    public static void RestartApp()
    {
        GameManager.Restart();
    }
}
