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

[RequireComponent(typeof(Button))]
public sealed class YearOption : MonoBehaviour
{
    public TextMeshProUGUI yearLabel;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ShowYear);
    }

    public void ShowYear()
    {
        CoreLogger.LogMessage($"Clicked on {yearLabel.text}");
    }
}
