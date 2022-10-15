/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SW.Logger;
using UnityEngine;
using UnityEngine.Networking;

namespace HWAPI
{
    public static class Parser<T>
    {
        public static List<VersionsTableCarInfo> FromWikitext(string wikiText)
        {
            List<string> lines = new(wikiText.Split("|-"));
            List<VersionsTableCarInfo> entries = new();

            // for (int i = 0; i < lines.Count; i++)
            // {
            //     Debug.Log(lines[i]);
            // }
            // Debug.Log("---------");

            lines.ForEach(line => string.Concat(line.Where(c => !char.IsWhiteSpace(c))));

            // for (int i = 0; i < lines.Count; i++)
            // {
            //     Debug.Log(lines[i]);
            // }
            // Debug.Log("---------");

            foreach (string line in lines)
            {
                string newline = line.Trim();
                if (newline.StartsWith("|") && !newline.StartsWith("|}"))
                {
                    string[] carInfo = newline.Split("\n");

                    for (int carInfoIndex = 0; carInfoIndex < carInfo.Length; carInfoIndex++)
                    {
                        // remove line declaration character
                        if (carInfo[carInfoIndex].StartsWith("|"))
                            carInfo[carInfoIndex] = carInfo[carInfoIndex][1..];

                        // replace blank line with no info text
                        if (carInfo[carInfoIndex]?.Length <= 0)
                            carInfo[carInfoIndex] = "No info";
                    }

                    Debug.Log("---------------");
                    for (int i = 0; i < carInfo.Length; i++)
                    {
                        Debug.Log(i + ": " + carInfo[i]);
                    }
                    Debug.Log("---------------");

                    // remove 2 characters from the start of the string if it starts with "[["
                    if (carInfo[12].StartsWith("[["))
                        carInfo[12] = carInfo[12][2..];

                    // remove 2 characters from the end of the string if it ends with "]]"
                    if (carInfo[12].EndsWith("]]"))
                        carInfo[12] = carInfo[12][..^2];

                    // check if raw car data can be splited, otherwise use the template
                    string[] carInfoPhotoProperties = { "File:Image_Not_Available.jpg" , "50" };
                    if (carInfo[12].Contains("|"))
                        carInfoPhotoProperties = carInfo[12].Split("|");

                    // remove 2 last characters if the string ends with "px"
                    if (carInfoPhotoProperties[1].EndsWith("px"))
                        carInfoPhotoProperties[1] = carInfoPhotoProperties[1][..^2];

                    // request the url for the new car info photo name
                    carInfo[12] = $"https://hotwheels.fandom.com/api.php?format=json&action=query&titles={UnityWebRequest.EscapeURL(carInfoPhotoProperties[0].Replace(" ", "_"))}&prop=pageimages&pithumbsize={carInfoPhotoProperties[1]}";

                    // create the new car info
                    VersionsTableCarInfo newCarInfo = new
                    (
                        colNumber:      carInfo[0],
                        year:           carInfo[1],
                        series:         carInfo[2],
                        color:          carInfo[3],
                        tampo:          carInfo[4],
                        baseColor:      carInfo[5],
                        windowColor:    carInfo[6],
                        interiorColor:  carInfo[7],
                        wheelType:      carInfo[8],
                        toyNumber:      carInfo[9],
                        country:        carInfo[10],
                        notes:          carInfo[11],
                        photo:          carInfo[12]
                    );

                    entries.Add(newCarInfo);
                }
            }

            return entries;
        }

        // TODO_HIGH: Finish this parser
        public static List<T> FromWikiText(string wikiText)
        {
            return null;
        }
    }
}
