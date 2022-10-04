/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using SW.Logger;
using HWAPI;

public abstract class MultiPageNavigation : MonoBehaviour
{
    [Header("Multi page navigation")]
    public Button previousPageBtn;
    public Button nextPageBtn;
    public TextMeshProUGUI pageIndexLbl;
    [Space(10.0f)]

    [HideInInspector]
    public YearCategoriesModel yearPages;

    private int currentPageIndex;
    private readonly Dictionary<int, string> pagesReferenceIdDict = new();
    private string dictPageId;
    private bool hasFoundLastPage;
    private int lastPageIndex;
    private bool hasSetLastPageIndex;

    public virtual void Awake()
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

    public virtual void CalculateButtonStates()
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

    public abstract void PageBehaviour();

    private void RequestPage(string cmcontinue)
    {
        Request.GetYearPage
        (
            cmcontinue,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (data) =>
            {
                yearPages = data;

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

                PageBehaviour();

                pageIndexLbl.text = $"{currentPageIndex + 1}";
            }
        );
    }
}
