/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using APIRequests;
using SW.Logger;
using UnityEngine;
using UnityEngine.UI;

public sealed class SpecificYearPanel : MonoBehaviour
{
    [Header("Content")]
    [SerializeField]
    private ShortCarInfoButtonUI shortCarInfoButtonPrefab;
    [SerializeField]
    private RectTransform contentRectTransform;
    [SerializeField]
    private Button backBtn;

    private void Awake()
    {
        if (contentRectTransform.childCount > 0)
        {
            CoreLogger.LogMessage("Deleting short car info buttons...");
            for (int childIndex = 0; childIndex < contentRectTransform.childCount; childIndex++)
            {
                Destroy(contentRectTransform.GetChild(childIndex));
            }
        }

        backBtn.onClick.AddListener(() => {});//PanelManager.SetHomePageVisible());


    }

    public void SetData(string data)
    {
        
    }
}
