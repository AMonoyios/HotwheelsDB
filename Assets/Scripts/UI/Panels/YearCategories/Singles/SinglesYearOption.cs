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

        GetCarsOfYear((cars) =>
        {
            for (int i = 0; i < cars.Count; i++)
            {
                Debug.Log($"Name: {cars[i].Name}, Image: {cars[i].Image}");
            }
        });
    }

    // FIXME: Car names do not match car image
    private void GetCarsOfYear(System.Action<List<SinglesPageSectionModel.Car>> onComplete)
    {
        Request.GetSinglesPage
        (
            page: page,
            onError: (error) =>
                CoreLogger.LogError(error)
            ,
            onSuccess: (data) =>
            {
                List<SinglesPageSectionModel.Car> Cars = new();
                for (int sectionIndex = 0; sectionIndex < data.PageSections.Count; sectionIndex++)
                {
                    Request.GetSinglesPageSection
                    (
                        page: page,
                        section: sectionIndex,
                        onError: (error) =>
                            CoreLogger.LogError(error),
                        onSuccess: (data) =>
                            {
                                for (int carIndex = 0; carIndex < data.TotalCars; carIndex++)
                                {
                                    Cars.Add(data.GetCar(carIndex));
                                }

                                onComplete(Cars);
                            }
                    );
                }
            }
        );
    }
}
