using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using PD.Logger;

public sealed class NotificationsManager : MonoPersistentSingleton<NotificationsManager>
{
    protected override void Awake()
    {
        if (AndroidNotificationCenter.Initialize())
        {
            CoreLogger.LogMessage("Initialized: Android notification center successfully.");
        }
        else
        {
            CoreLogger.LogError("Initialized Android notification center FAILED!");
        }
    }
}
