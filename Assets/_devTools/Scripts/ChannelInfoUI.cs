/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///     [What does this ChannelInfoUI do]
/// </summary>
public sealed class ChannelInfoUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    private Button deleteChannelBtn;

    [Header("Panels")]
    [SerializeField]
    private TextMeshProUGUI idTxt;
    [SerializeField]
    private TextMeshProUGUI nameTxt;
    [SerializeField]
    private TextMeshProUGUI descriptionTxt;
    [SerializeField]
    private TextMeshProUGUI importanceTxt;

    private void Awake()
    {
        deleteChannelBtn.onClick.AddListener(() => DeleteChannel());
    }

    public void DeleteChannel()
    {
        PD.Android.AndroidNotifications.DeleteChannelByID(idTxt.text);
        Destroy(gameObject);
    }

    public void InitChannel(Unity.Notifications.Android.AndroidNotificationChannel channel)
    {
        idTxt.text += channel.Id;
        nameTxt.text += channel.Name;
        descriptionTxt.text += channel.Description;
        importanceTxt.text += channel.Importance.ToString();
    }
}
