/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class APITests : MonoBehaviour
{
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
}
