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

    [ListEnumPopup(typeof(TestingAPIRequests), "tmp_stringsPopupList"), SerializeField]
    private string testCar;

    [SerializeField]
    private Button testCarBtn;

    [SerializeField]
    private Image image;

    private void Start()
    {
        testCarBtn.onClick.AddListener(() =>
        {
            CoreUtilities.ClearConsole(name);
            TestGetImage();
        });
    }

    public void TestGetImage()
    {
        Request.GetCarPageSections(testCar,
            onError: (error) => CoreLogger.LogError($"Failed to find sections for {testCar} page. {error}"),
            onSuccess: (sections) =>
            {
                int sectionIndex = Utils.GetIndexOfSection("Versions", sections);
                if (sectionIndex < 0)
                {
                    CoreLogger.LogError($"Versions section for {testCar} was not found!");
                    return;
                }

                Request.GetCarVersionsTable(testCar, sectionIndex,
                    onError: (error) => CoreLogger.LogError($"Failed to get {testCar} versions table. {error}"),
                    onSuccess: (versions) =>
                    {
                        // getting the first car only for testing purposes
                        VersionsTableCarInfo car = versions[0];

                        Request.GetSprite(car.Photo,
                            1080,
                            onError: (error) => image.sprite = error,
                            onSuccess: (success) => image.sprite = success
                        );
                    }
                );
            }
        );
    }

    public void TestSourceQuery(string query)
    {
        Request.GetJsonData(query,
            onError: (error) => CoreLogger.LogError(error),
            onSuccess: (photo) => CoreLogger.LogMessage(photo)
        );
    }

    public void TestCarVersionsTable()
    {
        Request.GetCarPageSections(testCar,
            onError: (error) => CoreLogger.LogError($"Failed to find sections for {testCar} page. {error}"),
            onSuccess: (sections) =>
            {
                int sectionIndex = Utils.GetIndexOfSection("Versions", sections);
                if (sectionIndex < 0)
                {
                    CoreLogger.LogError($"Versions section for {testCar} was not found!");
                    return;
                }

                Request.GetCarVersionsTable(testCar, sectionIndex,
                    onError: (error) => CoreLogger.LogError($"Failed to get {testCar} versions table. {error}"),
                    onSuccess: (versions) =>
                    {
                        for (int i = 0; i < versions.Count; i++)
                        {
                            CoreLogger.LogMessage($"Col#: {versions[i].ColNumber} \n" +
                                $"Year: {versions[i].Year} \n" +
                                $"Series: {versions[i].Series} \n" +
                                $"Color: {versions[i].Color} \n" +
                                $"Tampo: {versions[i].Tampo} \n" +
                                $"Base color: {versions[i].BaseColor} \n" +
                                $"Window color: {versions[i].WindowColor} \n" +
                                $"Interior color: {versions[i].InteriorColor} \n" +
                                $"Wheel type: {versions[i].WheelType} \n" +
                                $"Toy#: {versions[i].ToyNumber} \n" +
                                $"Country: {versions[i].Country} \n" +
                                $"Notes: {versions[i].Notes} \n" +
                                $"Photo: {versions[i].Photo}");
                        }
                    }
                );
            }
        );
    }
}
