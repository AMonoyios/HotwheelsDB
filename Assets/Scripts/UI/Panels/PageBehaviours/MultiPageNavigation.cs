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

    public abstract void PageBehaviour(T data);

    private void RequestPage(string cmcontinue)
    {
        Request.GetYearPage<T>
        (
            cmcontinue,
            onError: (error) =>
            {
                CoreLogger.LogError(error);
                OnComplete();
            },
            onSuccess: (data) =>
            {
                if (data.Navigate == null)
                {
                    hasFoundLastPage = true;
                    CalculateButtonStates();
                }
                else if (!pagesReferenceIdDict.ContainsValue(data.Navigate.next))
                {
                    if (pagesReferenceIdDict.TryAdd(currentPageIndex, data.Navigate.next))
                    {
                        CoreLogger.LogMessage($"Succesfully added {data.Navigate.next} to dictionary with key {currentPageIndex}");
                    }
                    else
                    {
                        CoreLogger.LogError($"Failed to add {data.Navigate.next} to dictionary with key {currentPageIndex}");
                    }
                }
                else
                {
                    CoreLogger.LogWarning($"Page with reference id {data.Navigate.next} already exists in {nameof(pagesReferenceIdDict)}", true);
                }

                PageBehaviour(data);

                pageIndexLbl.text = $"{currentPageIndex + 1}";

                OnComplete();
            }
        );
    }
}
