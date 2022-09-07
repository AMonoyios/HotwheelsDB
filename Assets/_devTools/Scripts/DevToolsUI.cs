/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PD.Logger;

/// <summary>
///     Manages all dev actions, this is a MonoPersistentSingleton.
/// </summary>
public sealed class DevToolsUI : MonoPersistentSingleton<DevToolsUI>
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private Button closeBtn;

#region Menu
    [Header("Menu")]
    [SerializeField]
    private RectTransform menu;
    [SerializeField]
    private Button menuShowBtn;
    [SerializeField]
    private Button menuHideBtn;
#endregion

#region Buttons
    [Header("Menu buttons")]
    [SerializeField]
    private Button showBtn;
    [SerializeField]
    private Button generalBtn;
    [SerializeField]
    private Button notificationsBtn;
    [SerializeField]
    private Button startupBtn;
    [SerializeField]
    private Button addressablesBtn;
    [SerializeField]
    private Button consoleBtn;
    [SerializeField]
    private Button closeMenuBtn;
#endregion

#region Panels
    [Header("Panels")]
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject generalPanel;
    [SerializeField]
    private GameObject notificationsPanel;
    [SerializeField]
    private GameObject startupPanel;
    [SerializeField]
    private GameObject addressablesPanel;
    [SerializeField]
    private GameObject consolePanel;
#endregion

#region Other
    [Header("Other")]
    [SerializeField]
    private TextMeshProUGUI gameModeStatusTxt;
    [SerializeField]
    private DevConsolePanelUI consolePanelUI;

    private static bool IsVisible => Instance.mainPanel.activeSelf;
#endregion

    private void Start()
    {
        Application.logMessageReceived += consolePanelUI.OnLogMessageReceived;

        if (GameManager.IsDevMode)
            CoreLogger.LogMessage("Game mode: DEV MODE");
        else
            CoreLogger.LogMessage("Game mode: Normal MODE");
    }

    protected override void OnDestroy()
    {
        // This is to prevent exception errors.
        // Because unity destroys gameobjects randomly when closing game we cannot control if
        // the gameobject that fires the log exists or if it has its parent destroyed, or even
        // having the main log manager (this) destroyed before the log was succesfully completed
        // the cycle.
        Application.logMessageReceived -= consolePanelUI.OnLogMessageReceived;
    }

    protected override void Awake()
    {
        base.Awake();
        if (this == null || gameObject == null)
            return;

        InitGameMode();

        if (GameManager.IsDevMode)
        {
            showBtn.onClick.AddListener(() => ShowHide());
            generalBtn.onClick.AddListener(() => ShowPanel(generalPanel, "General"));
            notificationsBtn.onClick.AddListener(() => ShowPanel(notificationsPanel, "Notifications"));
            startupBtn.onClick.AddListener(() => ShowPanel(startupPanel, "Startup"));
            addressablesBtn.onClick.AddListener(() => ShowPanel(addressablesPanel, "Addressables"));
            consoleBtn.onClick.AddListener(() => ShowPanel(consolePanel, "Console"));

            closeMenuBtn.onClick.AddListener(() => SetMenuVisible(false));

            mainPanel.SetActive(false);
            HideAllPanels();

            closeBtn.onClick.AddListener(() => ShowHide());

            menuShowBtn.onClick.AddListener(() => SetMenuVisible(true));
            menuHideBtn.onClick.AddListener(() => SetMenuVisible(false));
            SetMenuVisible(false);
        }
    }

    private void Update()
    {
        UpdateVisible();

        gameModeStatusTxt.text = "Test text";
    }

    private void InitGameMode()
    {
        GameManager.SetDevMode(false);
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
            GameManager.SetDevMode(true);
        #endif
    }

    private void UpdateVisible()
    {
        bool canShowDevBtn = GameManager.IsDevMode && !IsVisible;
        if (showBtn.gameObject.activeSelf != canShowDevBtn)
        {
            showBtn.gameObject.SetActive(canShowDevBtn);
            if(!GameManager.IsDevMode)
            {
                mainPanel.SetActive(false);
            }
        }
    }

    public void SetMenuVisible(bool visible)
    {
        menu.gameObject.SetActive(visible);
    }

    public void ShowHide()
    {
        Instance.HideAllPanels();
        Instance.mainPanel.SetActive(!IsVisible);

        if (Instance.mainPanel.activeSelf)
        {
            Instance.ShowPanel(generalPanel, "General");
        }
    }

    private void HideAllPanels()
    {
        title.text = "";

        generalPanel.SetActive(false);
        notificationsPanel.SetActive(false);
        startupPanel.SetActive(false);
        addressablesPanel.SetActive(false);
        consolePanel.SetActive(false);
    }

    public void ShowPanel(GameObject panel, string title)
    {
        HideAllPanels();
        panel.SetActive(true);

        this.title.text = title;

        SetMenuVisible(false);
    }
}
