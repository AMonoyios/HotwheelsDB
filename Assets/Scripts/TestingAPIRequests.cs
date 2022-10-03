/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HWAPI;
using SW.Logger;
using UnityEngine.UI;
using System.Reflection;
using SW.Utils;
using SW.Utils.Convertions;
using System.Text.RegularExpressions;
using TMPro;

/*
f(signature) -> page, next signature
f() -> page1, signature2
f(signature2) -> page2, signature3
*/

public sealed class TestingAPIRequests : MonoBehaviour
{
    [SerializeField]
    private Button previousPageBtn;
    // [SerializeField]
    // private string previousPage;
    [SerializeField]
    private Button nextPageBtn;
    // [SerializeField]
    // private string nextPage;
    [SerializeField]
    private TextMeshProUGUI pageIndexLbl;

    [Space(20.0f)]

    [SerializeField]
    private int currentPageIndex = 0;
    [SerializeField]
    private List<string> pagesSubcat = new();

    private void Awake()
    {
        RequestPage(string.Empty);

        previousPageBtn.onClick.AddListener(() =>
        {
            if (currentPageIndex <= 0)
            {
                return;
            }

            currentPageIndex--;

            if (currentPageIndex - 1 >= 0)
            {
                RequestPage(pagesSubcat[currentPageIndex - 1]);
            }
            else
            {
                RequestPage(string.Empty);
            }
        });
        nextPageBtn.onClick.AddListener(() =>
        {
            RequestPage(pagesSubcat[currentPageIndex]);
            currentPageIndex++;
        });
    }

    private void RequestPage(string cmcontinue)
    {
        CoreUtilities.ClearConsole(name);

        Request.GetYearPage
        (
            cmcontinue,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (yearPages) =>
            {
                if (yearPages.Navigate != null && !pagesSubcat.Contains(yearPages.Navigate.next))
                {
                    pagesSubcat.Add(yearPages.Navigate.next);
                }

                Debug.Log("-----------");
                for (int i = 0; i < yearPages.YearCategories.Count; i++)
                {
                    Debug.Log($"page label: {yearPages.YearCategories[i].label}");
                    Debug.Log($"page title: {yearPages.YearCategories[i].title}");
                    Debug.Log("~~~");
                }

                pageIndexLbl.text = $"{currentPageIndex + 1}";
            }
        );
    }

    #region Old methods
        // Request.GetCarPageSections(testCar,
        //     onError: (error) => CoreLogger.LogError($"Failed to find sections for {testCar} page. {error}"),
        //     onSuccess: (sections) =>
        //     {
        //         int sectionIndex = Utils.GetIndexOfSection("Versions", sections);
        //         if (sectionIndex < 0)
        //         {
        //             CoreLogger.LogError($"Versions section for {testCar} was not found!");
        //             return;
        //         }

        //         Request.GetCarVersionsTable(testCar, sectionIndex,
        //             onError: (error) => CoreLogger.LogError($"Failed to get {testCar} versions table. {error}"),
        //             onSuccess: (versions) =>
        //             {
        //                 // getting the first car only for testing purposes
        //                 print($"XXX versions count: {versions.Count}");
        //                 VersionsTableCarInfo car = versions[0];

        //                 Request.GetSprite(car.Photo,
        //                     1080,
        //                     onError: (error) => image.sprite = error,
        //                     onSuccess: (success) => image.sprite = success
        //                 );
        //             }
        //         );
        //     }
        // );
        #endregion

    public void TestSourceQuery(string query)
    {
        Request.GetJsonData(query,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (photo) => CoreLogger.LogMessage(photo)
        );
    }
}
