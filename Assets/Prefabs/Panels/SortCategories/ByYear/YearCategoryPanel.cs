/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using APIRequests;
using SW.Logger;
using Newtonsoft.Json;
using System.Linq;

public sealed class YearCategoryPanel : MonoBehaviour
{
    [Header("Content")]
    [SerializeField]
    private YearOptionButtonUI yearButtonPrefab;
    [SerializeField]
    private RectTransform byYearContentRectTransform;

    [Header("Other")]
    [SerializeField]
    private RectTransform byYearPanelRectTranform;

    private const float labelHeight = 150.0f;
    private float contentHeight;

    private void Awake()
    {
        if (byYearContentRectTransform.childCount > 0)
        {
            CoreLogger.LogMessage("Deleting year category button options");
            for (int childIndex = 0; childIndex < byYearContentRectTransform.childCount; childIndex++)
            {
                Destroy(byYearContentRectTransform.GetChild(childIndex));
            }
        }

        Request.RawData(Queries.HotWheelsBy100FirstYears,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (success) =>
            {
                CoreLogger.LogMessage("Fetched categories for first 100 years.");

                YearCategory yearCategories = JsonConvert.DeserializeObject<YearCategory>(success);
                yearCategories.query.categorymembers.Sort((x, y) => string.Compare(x.title, y.title));

                for (int i = 0; i < yearCategories.query.categorymembers.Count; i++)
                {
                    YearOptionButtonUI yearBtnUI = Instantiate(yearButtonPrefab, byYearContentRectTransform.transform);

                    YearCategory.CategoryMember categoryMember = yearCategories.query.categorymembers[i];
                    yearBtnUI.name = $"Option_{categoryMember.title}_Btn";

                    string year = new(categoryMember.title.Where(char.IsDigit).ToArray());
                    yearBtnUI.SetYearLabel(year);
                    yearBtnUI.SetYearURL(categoryMember.Url);
                }

                StartCoroutine(InitHeight());
            }
        );
    }

    private IEnumerator InitHeight()
    {
        yield return new WaitForSeconds(0.5f);

        contentHeight = byYearContentRectTransform.sizeDelta.y;
        byYearPanelRectTranform.sizeDelta = new(byYearPanelRectTranform.rect.width, labelHeight + contentHeight);
    }
}
