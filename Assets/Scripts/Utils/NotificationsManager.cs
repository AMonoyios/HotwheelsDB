/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

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
