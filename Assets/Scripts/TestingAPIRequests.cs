/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HWAPI;
using SW.Logger;

public sealed class TestingAPIRequests : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Request.CarVersionsTable(Queries.CarVersionsTable(),
        onError: (error) => CoreLogger.LogError(error),
        onSuccess: (success) =>
        {
            for (int i = 0; i < success.Count; i++)
            {
                Debug.Log($"Col#: {success[i].ColNumber} \n" +
                          $"Year: {success[i].Year} \n" +
                          $"Series: {success[i].Series} \n" +
                          $"Color: {success[i].Color} \n" +
                          $"Tampo: {success[i].Tampo} \n" +
                          $"Base color: {success[i].BaseColor} \n" +
                          $"Window color: {success[i].WindowColor} \n" +
                          $"Interior color: {success[i].InteriorColor} \n" +
                          $"Wheel type: {success[i].WheelType} \n" +
                          $"Toy#: {success[i].ToyNumber} \n" +
                          $"Country: {success[i].Country} \n" +
                          $"Notes: {success[i].Notes} \n" +
                          $"Photo: {success[i].Photo}");
            }
        });
    }
}
