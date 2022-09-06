/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PD.Networking;
using PD.Logger;

public class TestingScript : MonoBehaviour
{
    [Header("Raw Data")]
    [SerializeField]
    private TextMeshProUGUI rawDataOutput;

    [Header("Find String")]
    [SerializeField]
    private TextMeshProUGUI findStringTMP;

    [Header("Image Response")]
    [SerializeField]
    private Image responseImage;
    [SerializeField]
    private TextMeshProUGUI responseImageTMP;

    private void Start()
    {
        CoreRequest.Init();
    }

    public void FetchRawDataFromURL(TMP_InputField input)
    {
        CoreRequest.GetRawDataFrom(input.text,
            (string error) =>
            {
                CoreLogger.LogError(error);
                rawDataOutput.text = error;
            },
            (string success) =>
            {
                CoreLogger.LogMessage($"Succesfully fetched raw data from URL {input.text}.");
                rawDataOutput.text = success;
            }
        );
    }

    /// <summary>
    ///     Finds a string in text using Aho-Corasick algorithm.
    ///     Is used in SampleScene > FindStringButton.
    /// </summary>
    /// <param name="input">
    ///     Target TMP text field.
    /// </param>
    /// <returns>
    ///     Will return a color depending if string was found or not.
    /// </returns>
    public void FindString(TMP_InputField input)
    {
        Image inputImage = input.GetComponent<Image>();

        if (input.text?.Length == 0 || input.text == null)
        {
            CoreLogger.LogWarning("Input field was Null or Empty, Algorithm did not run.");
            inputImage.color = Color.white;
        }
        else
        {
            string[] words = input.text.Split(' ');

            if (CoreRequest.DoesStringExist(findStringTMP.text, words))
            {
                CoreLogger.LogMessage($"Word(s) {input.text} found in the above RawData.");
                inputImage.color = Color.green;
            }
            else
            {
                CoreLogger.LogMessage($"Word(s) {input.text} found in the above RawData.");
                inputImage.color = Color.red;
            }
        }
    }

    public void FetchSpriteFromURL(TMP_InputField input)
    {
        CoreRequest.GetSprite(input.text,
            (string error) =>
            {
                CoreLogger.LogError(error);
                responseImage.sprite = null;
                responseImageTMP.text = error;
            },
            (Sprite success) =>
            {
                CoreLogger.LogMessage($"Sprite {success.name} was successfully fetched from {input.text}.");
                responseImage.sprite = success;
                responseImageTMP.text = string.Empty;
            }
        );
    }
}
