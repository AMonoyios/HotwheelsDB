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

public sealed class TestingAPIRequests : MonoBehaviour, ISerializationCallbackReceiver
{
    #region StringEnumPopup
    public static List<string> tmp_stringsPopupList;
    [HideInInspector] public List<string> StringPopupList;
    public void UpdateList() { tmp_stringsPopupList = StringPopupList; }
    public void OnBeforeSerialize() { UpdateList(); }
    public void OnAfterDeserialize() {}
    #endregion

    [ContextMenu("Create test cars names list")]
    public void CreateCarNamesList()
    {
        StringPopupList = new()
        {
            "2001_BMW_M5_E39",
            "BMW_M3_E46",
            "Mercedes-Benz_CL55_AMG",
            "%2716_Cadillac_ATS-V_R",
            "Sakura_Sprinter",
            "Nissan_Silvia_(S13)"
        };
    }

    // [ListEnumPopup(typeof(TestingAPIRequests), "tmp_stringsPopupList"), SerializeField]
    // private string testCar;

    [SerializeField]
    private Button previousPageBtn;
    [SerializeField]
    private TMPro.TextMeshProUGUI pageLabel;
    [SerializeField]
    private Button nextPageBtn;

    [SerializeField]
    private uint pageIndex;

    private void Awake()
    {
        CalculatePage(pageIndex);

        previousPageBtn.onClick.AddListener(() => CalculatePage(pageIndex--));
        nextPageBtn.onClick.AddListener(() => {Debug.Log("Before page index: "+ pageIndex); CalculatePage(pageIndex++);});

        previousPageBtn.interactable = false;
    }

    public void CalculatePage(uint pageIndex)
    {
        Debug.Log("After page index: " + pageIndex);
        pageLabel.text = pageIndex.ToString();

        Request.GetYears
        (
            pageIndex,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (yearPages) =>
            {
                previousPageBtn.interactable = pageIndex != 1;

                if (yearPages.Count == 0)
                {
                    nextPageBtn.interactable = false;
                }
                else
                {
                    nextPageBtn.interactable = true;

                    CoreUtilities.ClearConsole(name);

                    Debug.Log("-----------");
                    for (int i = 0; i < yearPages.Count; i++)
                    {
                        Debug.Log(yearPages[i].title);
                        Debug.Log(yearPages[i].label);
                        Debug.Log(yearPages[i].onPage);
                        Debug.Log("~");
                    }
                    Debug.Log("-----------");
                }
            }
        );

        #region obsolete
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
    }

    public void TestSourceQuery(string query)
    {
        Request.GetJsonData(query,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (photo) => CoreLogger.LogMessage(photo)
        );
    }
}
