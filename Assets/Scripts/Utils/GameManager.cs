/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using SW.Logger;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Responsible to hold basic states and actions of game.
/// </summary>
public sealed class GameManager : MonoPersistentSingleton<GameManager>
{
    [SerializeField]
    private bool forceDevMode;

    protected override void Awake()
    {
        if (AndroidNotificationCenter.Initialize())
            CoreLogger.LogMessage("Initialized: Android notification center successfully.");
        else
            CoreLogger.LogError("Initialized Android notification center FAILED!");

        if (forceDevMode)
        {
            SetDevMode(true);
        }
    }

    private static bool inDevMode = false;
    /// <summary>
    ///     Sets the dev mode
    /// </summary>
    public static void SetDevMode(bool state)
    {
        inDevMode = state;
    }

    /// <summary>
    ///     Will return the current state of the game.
    /// </summary>
    public static bool IsDevMode
    {
        get
        {
            return inDevMode;
        }
    }

    public static string GetLocalTime()
    {
        return DateTime.UtcNow.ToLongTimeString();
    }

    public static string GetLocalDate()
    {
        return DateTime.UtcNow.ToLongDateString();
    }

    /// <summary>
    ///     Restarts the game
    /// </summary>
    public static void Restart()
    {
        // Start from splash
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));

        // if in dev mode and having it open close it
        if (IsDevMode && DevToolsUI.instance.isActiveAndEnabled)
            DevToolsUI.instance.ShowHide();

        // Close all scenes
        for(int i = 0; i < SceneManager.sceneCount; i ++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.IsValid() && scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    /// <summary>
    ///     Quits game
    /// </summary>
    public static void Quit()
    {
        Application.Quit();
    }
}
