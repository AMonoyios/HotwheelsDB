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
    [Header("HTML")]
    [SerializeField]
    private TextMeshProUGUI responseText;

    [Header("Image")]
    [SerializeField]
    private Image responseImage;
    [SerializeField]
    private TextMeshProUGUI responseTextureText;

    [Header("Find")]
    [SerializeField]
    private TMP_InputField findInputTextField;
    private Image findInputTextFieldImage;

    private static string GetHomeUrl => "https://hotwheels.fandom.com/wiki/Hot_Wheels";
    private static string GetStatisticsUrl => "https://hotwheels.fandom.com/wiki/Special:Statistics";
    private static string GetHWLogoImageUrl => "https://static.wikia.nocookie.net/hotwheels/images/e/e6/Site-logo.png/revision/latest?cb=20210601150811";
    private static string GetStatisticsTableID => "wikitable mw-statistics-table";

    private void Start()
    {
        if (findInputTextField != null)
        {
            findInputTextFieldImage = findInputTextField.GetComponent<Image>();
        }
        else
        {
            CoreLogger.LogError("Error: findInputTextField is null", gameObjectSource: gameObject);
        }

        CoreRequest.Init();

        CoreRequest.GetHTML(GetStatisticsUrl,
            (string error) =>
            {
                CoreLogger.LogError($"Error - [HTML]: {error}");
                responseText.text = error;
            },
            (string success) =>
            {
                CoreLogger.LogMessage("Received - [HTML]");
                responseText.text = success;
            }
        );

        CoreRequest.GetSprite(GetHWLogoImageUrl,
            (string error) =>
            {
                CoreLogger.LogError($"Error - [Sprite]: {error}");
                responseImage.sprite = null;
                responseTextureText.text = error;
            },
            (Sprite success) =>
            {
                CoreLogger.LogMessage($"Received - [Sprite]: {success}");
                responseImage.sprite = success;
                responseTextureText.text = string.Empty;
            }
        );
    }

    public void FindString(TMP_InputField input)
    {
        if (input.text?.Length == 0 || input.text == null)
        {
            CoreLogger.LogWarning("Input field was Null or Empty, Algorithm did not run.");
            findInputTextFieldImage.color = Color.white;
        }
        else
        {
            string[] words = input.text.Split(' ');

            if (CoreRequest.DoesStringExist(responseText.text, words))
            {
                CoreLogger.LogMessage("Word found in the above HTML.");
                findInputTextFieldImage.color = Color.green;
            }
            else
            {
                CoreLogger.LogMessage("Word was not found in the HTML.");
                findInputTextFieldImage.color = Color.red;
            }
        }
    }
}
