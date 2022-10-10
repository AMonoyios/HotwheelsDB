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
    [SerializeField]
    private TextMeshProUGUI yearLabel;

    private string title;
    private string page;

    public void Init(string label, string title, string page)
    {
        yearLabel.text = label;
        this.title = title;
        this.page = page;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ShowYear);
    }

    public void ShowYear()
    {
        CoreLogger.LogMessage($"Clicked on {yearLabel.text}, title: {title}, page: {page}");
    }
}
