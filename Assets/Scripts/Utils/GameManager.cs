/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Responsible to hold basic states and actions of game.
/// </summary>
public sealed class GameManager : MonoPersistentSingleton<GameManager>
{
    private static bool inDevMode = false;

    /// <summary>
    ///     Sets the dev mode
    /// </summary>
    public static void SetDevMode(bool devMode)
    {
        inDevMode = devMode;
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

    /// <summary>
    ///     Restarts the game
    /// </summary>
    public static void Restart()
    {
        // Close all scenes
        for(int i = 0; i < SceneManager.sceneCount; i ++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.IsValid() && scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // Start from splash
        SceneManager.LoadScene(0);
    }

    /// <summary>
    ///     Quits game
    /// </summary>
    public static void Quit()
    {
        Application.Quit();
    }
}
