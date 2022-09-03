using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using TMPro;
using System;
using UnityEngine.UI;

using PD.Utils.Convertions;

public class TestingScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI responseText;
    [SerializeField]
    private Image responseImage;
    [SerializeField]
    private TextMeshProUGUI responseTextureText;

    private static string GetJsonUrl => "https://hotwheels.fandom.com/wiki/Hot_Wheels";
    private static string GetImageUrl => "https://static.wikia.nocookie.net/hotwheels/images/e/e6/Site-logo.png/revision/latest?cb=20210601150811";

    private void Start()
    {
        GetJson(GetJsonUrl,
            (string error) =>
            {
                Debug.LogError($"Error: {error}");
                responseText.text = error;
            },
            (string success) =>
            {
                Debug.Log($"Received: {success}");
                responseText.text = success;
            }
        );

        GetSprite(GetImageUrl,
            (string error) =>
            {
                Debug.LogError($"Error: {error}");
                responseImage.sprite = null;
                responseTextureText.text = error;
            },
            (Sprite success) =>
            {
                Debug.Log($"Received: {success.name}");
                responseImage.sprite = success;
                responseTextureText.text = string.Empty;
            }
        );
    }

    private void GetJson(string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(GetJsonCoroutine(url, onError, onSuccess));
    }
    private IEnumerator GetJsonCoroutine(string url, Action<string> onError, Action<string> onSuccess)
    {
        using UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            onError(unityWebRequest.error);
        }
        else
        {
            onSuccess(unityWebRequest.downloadHandler.text);
        }
    }

    private void GetSprite(string url, Action<string> onError, Action<Sprite> onSuccess)
    {
        StartCoroutine(GetSpriteCoroutine(url, onError, onSuccess));
    }
    private IEnumerator GetSpriteCoroutine(string url, Action<string> onError, Action<Sprite> onSuccess)
    {
        using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url);
            yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            onError(unityWebRequest.error);
        }
        else
        {
            if (unityWebRequest.downloadHandler is not DownloadHandlerTexture downloadHandlerTexture)
            {
                onError("Failed to locate sprite from given url.");
            }
            else
            {
                onSuccess(CoreConvertions.ConvertTexture2DToSprite(downloadHandlerTexture.texture));
            }
        }
    }
}
