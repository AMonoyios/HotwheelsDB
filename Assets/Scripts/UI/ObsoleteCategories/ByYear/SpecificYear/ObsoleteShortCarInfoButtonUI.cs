/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using SW.Logger;
using APIRequests;

public sealed class ShortCarInfoButtonUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI carNameLabel;
    private string carUrl;

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            CoreLogger.LogMessage($"Requesting data from {carNameLabel.text}.");

            //Request.RawData(Queries.Hot)
        });
    }

    public void SetCarNameLabel(string name)
    {
        carNameLabel.text = name;
    }

    public void SetCarURL(string url)
    {
        carUrl = url;
    }
}
