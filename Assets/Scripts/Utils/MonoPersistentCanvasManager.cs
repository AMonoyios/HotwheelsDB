/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
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

    public void ShowConnectionErrorPopup()
    {
        contentParentTransform.gameObject.SetActive(true);
        connectionErrorPopupInstance = Instantiate(connectionErrorPopup, contentParentTransform);
    }

    public void HideConnectionErrorPopup(Action lastRequest)
    {
        if (connectionErrorPopupInstance != null)
        {
            Destroy(connectionErrorPopupInstance);
            contentParentTransform.gameObject.SetActive(false);
        }

        lastRequest?.Invoke();
    }
}
