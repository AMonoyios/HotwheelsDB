/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using SW.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class InitApp : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.LoadScene(2);

        // TODO: Develop a new way to check initial connection to api.
        // CoreNetworking.Init();
        // MonoPersistentCanvasManager.Instance.ShowLoadingPopup();
        // CoreNetworking.CheckConnection("https://hotwheels.fandom.com/wiki/Hot_Wheels",
        //                             CoreNetworking.GetConnectionRetryDelta,
        //                             CoreNetworking.GetMaxConnectionRetry,
        //                             onError: () => MonoPersistentCanvasManager.Instance.ShowConnectionErrorPopup(),
        //                             onSuccess: () => GameManager.Instance.LoadScene("Main"));
    }
}
