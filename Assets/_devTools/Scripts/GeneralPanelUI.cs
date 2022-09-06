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
    [SerializeField]
    private Button createTestChannelBtn;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => RestartApp());
        createTestChannelBtn.onClick.AddListener(() => CreateTestChannel());
        testNotificationButton.onClick.AddListener(() => TestNotification());
    }

    public static void RestartApp()
    {
        GameManager.Restart();
    }

    public static void TestNotification()
    {
        AndroidNotifications.CreateNotification("Test", "This is a test notification.", System.DateTime.Now.AddSeconds(15), NotificationChannelRepo.TestingChannelID);
    }

    public static void CreateTestChannel()
    {
        //TODO: when the code generation for automatic channel tracking is implemented replace this with just the string "TestingChannel" in the place of NotificationChannelRepo.TestingChannelID
        AndroidNotifications.CreateNotificationChannel(NotificationChannelRepo.TestingChannelID, "Testing Channel", "Channel created for testing purposes only.");
    }
}
