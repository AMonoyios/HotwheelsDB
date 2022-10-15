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
using HWAPI;

[RequireComponent(typeof(Button))]
public sealed class SinglesYearOption : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI yearLabel;

    [Space(10.0f)]
    [Header("Debug")]
    [SerializeField]
    private string title;
    [SerializeField]
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

        GetCarsOfYear();
    }

    // TODO_HIGH: Test this if is working
    private void GetCarsOfYear()
    {
        RequestSinglesPage.GetSinglesPage
        (
            page: page,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (singlesPageModel) =>
            {
                List<SinglesPageSectionModel.Car> cars = new();

                RequestSinglesPageSection.GetSinglesPageSection
                (
                    page: page,
                    singlesPageModel: singlesPageModel,
                    onError: (error) => CoreLogger.LogError(error),
                    onSuccess: (parseList) =>
                    {
                        for (int i = 0; i < parseList.Count; i++)
                        {
                            Debug.Log($"Car {i}: {parseList[i].title}");
                        }
                    }
                );
            }
        );
    }
}
