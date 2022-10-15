/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SW.Logger;

public abstract class MultiPageNavigation<T> : BasePanel where T : BaseNavigateModel
{
    [Header("Multi page navigation")]
    public Button previousPageBtn;
    public Button nextPageBtn;
    public TextMeshProUGUI pageIndexLbl;
    [Space(10.0f)]

    private int currentPageIndex;
    private readonly Dictionary<int, string> pagesReferenceIdDict = new();
    private string dictPageId;
    private bool hasFoundLastPage;
    private int lastPageIndex;
    private bool hasSetLastPageIndex;

    private void Awake()
    {
        Init();

        ShowFirstPage();

        previousPageBtn.onClick.AddListener(() =>
        {
            ShowPreviousPage();
            OnButtonClick();
        });
        nextPageBtn.onClick.AddListener(() =>
        {
            ShowNextPage();
            OnButtonClick();
        });
    }

    public virtual void Init()
    {
        // this is meant to be overwritten
    }

    public virtual void OnButtonClick()
    {
        // this is meant to be overwritten
    }

    public virtual void OnComplete()
    {
        // this is meant to be overwritten
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

    public abstract void PageBehaviour(HWAPI.YearCategoriesModel data);

    private void RequestPage(string cmcontinue)
    {
        // TODO_LOW: This currently supports ONLY YearPages to navigate.
        // Make a base navigation Model that hold all common navigation JsonProperties
        // then make all pages that have navigation derive from it.
        HWAPI.RequestYearPage.GetYearPage
        (
            cmcontinue: cmcontinue,
            onError: (error) =>
            {
                CoreLogger.LogError(error);
                OnComplete();
            },
            onSuccess: (yearPage) =>
            {
                if (yearPage.Navigate == null)
                {
                    hasFoundLastPage = true;
                    CalculateButtonStates();
                }
                else if (!pagesReferenceIdDict.ContainsValue(yearPage.Navigate.next))
                {
                    if (pagesReferenceIdDict.TryAdd(currentPageIndex, yearPage.Navigate.next))
                    {
                        CoreLogger.LogMessage($"Succesfully added {yearPage.Navigate.next} to dictionary with key {currentPageIndex}");
                    }
                    else
                    {
                        CoreLogger.LogError($"Failed to add {yearPage.Navigate.next} to dictionary with key {currentPageIndex}");
                    }
                }
                else
                {
                    CoreLogger.LogWarning($"Page with reference id {yearPage.Navigate.next} already exists in {nameof(pagesReferenceIdDict)}", true);
                }

                PageBehaviour(yearPage);

                pageIndexLbl.text = $"{currentPageIndex + 1}";

                OnComplete();
            }
        );
    }
}
