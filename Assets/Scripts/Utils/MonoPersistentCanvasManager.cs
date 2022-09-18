/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Threading.Tasks;
using SW.Logger;
using UnityEngine;

/// <summary>
///     [What does this MonoPersistentCanvasManager do]
/// </summary>
public sealed class MonoPersistentCanvasManager : MonoPersistentSingleton<MonoPersistentCanvasManager>
{
    [SerializeField]
    private Transform contentParentTransform;

    [Header("Popups")]
    [SerializeField]
    private GameObject connectionErrorPopup;
    private GameObject connectionErrorPopupInstance;

    [Header("Loading")]
    [SerializeField]
    private GameObject loadingPopup;
    private GameObject loadingPopupInstance;

    public void ShowConnectionErrorPopup()
    {
        if (connectionErrorPopupInstance == null)
        {
            contentParentTransform.gameObject.SetActive(true);
            connectionErrorPopupInstance = Instantiate(connectionErrorPopup, contentParentTransform);
        }
    }

    public void HideConnectionErrorPopup(Action lastRequest = null)
    {
        if (connectionErrorPopupInstance != null)
        {
            Destroy(connectionErrorPopupInstance);
            contentParentTransform.gameObject.SetActive(false);
        }

        lastRequest?.Invoke();
    }

    public void ShowLoadingPopup(Action execute)
    {
        if (loadingPopupInstance == null)
        {
            contentParentTransform.gameObject.SetActive(true);
            loadingPopupInstance = Instantiate(loadingPopup, contentParentTransform);
            CoreLogger.LogMessage("Showing loading popup.");
        }

        CoreLogger.LogMessage($"Executing {execute}.");
        execute();
    }
    public void HideLoadingPopup()
    {
        if (loadingPopupInstance != null)
        {
            Destroy(loadingPopupInstance);
            contentParentTransform.gameObject.SetActive(false);
            CoreLogger.LogMessage("Hiding loading popup.");
        }
    }
}
