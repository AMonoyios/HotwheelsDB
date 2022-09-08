/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SW.Networking;
using SW.Logger;
using Newtonsoft.Json;

public class TestingScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    private Button fetchRawDataBtn;
    [SerializeField]
    private Button findStringBtn;
    [SerializeField]
    private Button fetchImageBtn;

    [Header("Input fields")]
    [SerializeField]
    private TMP_InputField rawDataURLInputField;
    [SerializeField]
    private TMP_InputField findStringInputField;
    [SerializeField]
    private TMP_InputField imageURLInputField;

    [Header("Response")]
    [SerializeField]
    private TextMeshProUGUI rawDataOutput;
    [SerializeField]
    private Image responseImage;
    [SerializeField]
    private TextMeshProUGUI responseImageTMP;

    private void Start()
    {
        CoreNetworking.Init();

        AddButtonListeners();

        CoreNetworking.GetRawDataFrom(
            "https://hotwheels.fandom.com/api.php?action=parse&page=Bone_Shaker&format=json",
            (string error) => CoreLogger.LogError(error),
            (string result) => CoreLogger.LogMessage(JsonConvert.DeserializeObject<TestParseResponse>(result).ToString())
        );
    }

    private void AddButtonListeners()
    {
        fetchRawDataBtn.onClick.AddListener(() => FetchRawDataFromURL(rawDataURLInputField));
        findStringBtn.onClick.AddListener(() => FindString(findStringInputField));
        fetchImageBtn.onClick.AddListener(() => FetchSpriteFromURL(imageURLInputField));
    }

    /// <summary>
    ///     Fetching raw data from given URL.
    /// </summary>
    /// <param name="input">
    ///     The URL to fetch the raw data from.
    /// </param>
    public void FetchRawDataFromURL(TMP_InputField input)
    {
        CoreNetworking.GetRawDataFrom(input.text,
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

            if (CoreNetworking.DoesStringExist(findStringInputField.text, words))
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

    /// <summary>
    ///     Fetching an image from a given URL.
    /// </summary>
    /// <param name="input">
    ///     The image URL.
    /// </param>
    public void FetchSpriteFromURL(TMP_InputField input)
    {
        CoreNetworking.GetSprite(input.text,
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
