/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HWAPI;
using SW.Logger;
using SW.Utils;
using TMPro;

public sealed class TestingAPIRequests : MonoBehaviour
{
    [SerializeField]
    private Button previousPageBtn;
    [SerializeField]
    private Button nextPageBtn;
    [SerializeField]
    private TextMeshProUGUI pageIndexLbl;

    [Space(10.0f)]
    [Header("Debugging")]
    [SerializeField]
    private bool clearConsoleAfterRequest;
    [SerializeField]
    private int currentPageIndex;
    private readonly Dictionary<int, string> pagesReferenceIdDict = new();
    [SerializeField]
    private string dictPageId;
    [SerializeField]
    private bool hasFoundLastPage;
    [SerializeField]
    private int lastPageIndex;
    [SerializeField]
    private bool hasSetLastPageIndex;

    private void Awake()
    {
        ShowFirstPage();

        previousPageBtn.onClick.AddListener(() => ShowPreviousPage());
        nextPageBtn.onClick.AddListener(() => ShowNextPage());
    }

    private void ShowFirstPage()
    {
        currentPageIndex = 0;
        RequestPage(string.Empty);

        CalculateButtonStates();
    }

    private void ShowPreviousPage()
    {
        currentPageIndex--;

        if (currentPageIndex - 1 >= 0)
        {
            if (pagesReferenceIdDict.TryGetValue(currentPageIndex - 1, out dictPageId))
            {
                CoreLogger.LogMessage($"Found page id {dictPageId} in {nameof(pagesReferenceIdDict)} at index {currentPageIndex - 1}", true);
                RequestPage(dictPageId);

                CalculateButtonStates();
            }
            else
            {
                // FIXME: in all else case of try get the dictPageId is wrong or not the one you want to show
                CoreLogger.LogError($"Failed to find {dictPageId} in {nameof(pagesReferenceIdDict)} at index {currentPageIndex - 1}", true);
            }
        }
        else
        {
            ShowFirstPage();
        }
    }

    private void ShowNextPage()
    {
        if (pagesReferenceIdDict.TryGetValue(currentPageIndex, out dictPageId))
        {
            CoreLogger.LogMessage($"Found page id {dictPageId} in {nameof(pagesReferenceIdDict)} at index {currentPageIndex}", true);
            RequestPage(dictPageId);

            currentPageIndex++;

            CalculateButtonStates();
        }
        else
        {
            CoreLogger.LogError($"Failed to find {dictPageId} in {nameof(pagesReferenceIdDict)} at index {currentPageIndex}", true);
        }
    }

    private void CalculateButtonStates()
    {
        previousPageBtn.interactable = currentPageIndex > 0;
        CoreLogger.LogMessage($"{nameof(previousPageBtn)} was set to {(currentPageIndex > 0 ? "true" : "false")}", true);

        if (hasFoundLastPage)
        {
            nextPageBtn.interactable = currentPageIndex < lastPageIndex;
            CoreLogger.LogMessage($"Last page was found at index {currentPageIndex}", true);

            if (!hasSetLastPageIndex)
            {
                lastPageIndex = currentPageIndex;
                hasSetLastPageIndex = true;
                CoreLogger.LogMessage($"Last page index was set and locked, index: {lastPageIndex}", true);
            }
        }
    }

    private void RequestPage(string cmcontinue)
    {
        if (clearConsoleAfterRequest)
            CoreUtilities.ClearConsole(name);

        Request.GetYearPage
        (
            cmcontinue,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (yearPages) =>
            {
                if (yearPages.Navigate == null)
                {
                    hasFoundLastPage = true;
                    CalculateButtonStates();
                }
                else if (!pagesReferenceIdDict.ContainsValue(yearPages.Navigate.next))
                {
                    if (pagesReferenceIdDict.TryAdd(currentPageIndex, yearPages.Navigate.next))
                    {
                        CoreLogger.LogMessage($"Succesfully added {yearPages.Navigate.next} to dictionary with key {currentPageIndex}");
                    }
                    else
                    {
                        CoreLogger.LogError($"Failed to add {yearPages.Navigate.next} to dictionary with key {currentPageIndex}");
                    }
                }
                else
                {
                    CoreLogger.LogWarning($"Page with reference id {yearPages.Navigate.next} already exists in {nameof(pagesReferenceIdDict)}", true);
                }

                // ---------- This is where you show all the data for each page -------------- //

                // Debug.Log("-----------");
                // for (int i = 0; i < yearPages.YearCategories.Count; i++)
                // {
                //     Debug.Log($"page label: {yearPages.YearCategories[i].label}");
                //     Debug.Log($"page title: {yearPages.YearCategories[i].title}");
                //     Debug.Log("~~~");
                // }

                // --------------------------------------------------------------------------- //

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
