/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using APIRequests;
using SW.Logger;

public sealed class YearOptionButtonUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI yearLabel;
    private string yearUrl;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            CoreLogger.LogMessage($"Requesting data from {yearLabel.text}.");

            Request.RawData(Queries.HotWheelsYearCategory(yearUrl),
                onError: (error) => CoreLogger.LogError(error),
                onSuccess: (_) => CoreLogger.LogMessage($"Succesfully fetched data for {yearUrl}.")
            );
        });
    }

    public void SetYearLabel(string year)
    {
        yearLabel.text = year;
    }

    public void SetYearURL(string url)
    {
        yearUrl = url;
    }
}
