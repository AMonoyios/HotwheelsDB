/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PD.Android;
using PD.Logger;

/// <summary>
///     [What does this DevNotificationsPanelUI do]
/// </summary>
public sealed class DevNotificationsPanelUI : MonoBehaviour
{
    [Header("Channels")]
    [SerializeField]
    private TextMeshProUGUI channelCountTxt;
    [SerializeField]
    private TMP_InputField channelIDTxt;
    [SerializeField]
    private TMP_InputField channelTitleTxt;
    [SerializeField]
    private TMP_InputField channelDescriptionTxt;
    [SerializeField]
    private TMP_Dropdown channelImportanceDropDown;
    [SerializeField]
    private Button initNotificationCenterBtn;
    [SerializeField]
    private Button updateChannelsListBtn;
    [SerializeField]
    private Button newChannelBtn;
    [SerializeField]
    private GameObject channelInfoPrefab;
    [SerializeField]
    private Transform channelsListParentTransform;

    private void Awake()
    {
        initNotificationCenterBtn.onClick.AddListener(() => InitNotificationCenter());
        updateChannelsListBtn.onClick.AddListener(() => UpdateChannelsList());
        newChannelBtn.onClick.AddListener(() => CreateNewChannel(channelIDTxt, channelTitleTxt, channelDescriptionTxt, channelImportanceDropDown));

        UpdateChannelsList();
    }

    private void LateUpdate()
    {
        if (GameManager.IsDevMode && this.gameObject.activeInHierarchy)
        {
            newChannelBtn.interactable = channelIDTxt.text != "" && channelTitleTxt.text != "" && channelDescriptionTxt.text != "";
        }
    }

    public void InitNotificationCenter()
    {
        AndroidNotifications.InitializeAndroidNotificationCenter();
    }

    public void UpdateChannelsList()
    {
        channelCountTxt.text = AndroidNotifications.GetNotificationChannelCount().ToString();

        foreach (Transform channel in channelsListParentTransform)
        {
            CoreLogger.LogMessage($"Deleting {channel.name} channel info prefab...");
            Destroy(channel.gameObject);
        }

        for (int i = 0; i < AndroidNotifications.GetNotificationChannelCount(); i++)
        {
            GameObject newChannel = Instantiate(channelInfoPrefab, channelsListParentTransform);

            newChannel.GetComponent<ChannelInfoUI>().InitChannel(AndroidNotifications.GetAndroidNotificationChannelByIndex(i));
        }
    }

    public void CreateNewChannel(TMP_InputField id, TMP_InputField title, TMP_InputField desc, TMP_Dropdown importance)
    {
        AndroidNotifications.CreateNotificationChannel(id.text, title.text, desc.text, AndroidNotifications.GetImportanceByDropDown(importance));

        UpdateChannelsList();
    }
}
