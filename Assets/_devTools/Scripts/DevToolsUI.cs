/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using PD.Logger;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Manages all dev actions, this is a MonoPersistentSingleton.
/// </summary>
public sealed class DevToolsUI : MonoPersistentSingleton<DevToolsUI>
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private Button closeBtn;

    [Header("Menu")]
    [SerializeField]
    private RectTransform menu;
    [SerializeField]
    private Button menuShowBtn;
    [SerializeField]
    private Button menuHideBtn;

    [Header("Menu buttons")]
    [SerializeField]
    private Button showBtn;
    [SerializeField]
    private Button generalBtn;
    [SerializeField]
    private Button startupBtn;
    [SerializeField]
    private Button addressablesBtn;
    [SerializeField]
    private Button consoleBtn;
    [SerializeField]
    private Button closeMenuBtn;

    [Header("Panels")]
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject generalPanel;
    [SerializeField]
    private GameObject startupPanel;
    [SerializeField]
    private GameObject addressablesPanel;
    [SerializeField]
    private GameObject consolePanel;

    [Header("Other")]
    [SerializeField]
    private TextMeshProUGUI gameModeStatusTxt;
    [SerializeField]
    private ConsolePanelUI consolePanelUI;

    private static bool IsVisible => Instance.mainPanel.activeSelf;

    private void Start()
    {
        Application.logMessageReceived += consolePanelUI.OnLogMessageReceived;

        if (GameManager.IsDevMode)
            CoreLogger.LogMessage("Game mode: DEV MODE");
        else
            CoreLogger.LogMessage("Game mode: Normal MODE");
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
            generalBtn.onClick.AddListener(() => ShowGeneralPanel());
            startupBtn.onClick.AddListener(() => ShowStartupPanel());
            addressablesBtn.onClick.AddListener(() => ShowAddressablesPanel());
            consoleBtn.onClick.AddListener(() => ShowConsolePanel());

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

    public static void ShowHide()
    {
        Instance.HideAllPanels();
        Instance.mainPanel.SetActive(!IsVisible);

        if (Instance.mainPanel.activeSelf)
        {
            Instance.ShowGeneralPanel();
        }
    }

    private void HideAllPanels()
    {
        title.text = "";

        generalPanel.SetActive(false);
        startupPanel.SetActive(false);
        addressablesPanel.SetActive(false);
        consolePanel.SetActive(false);
    }

    public void ShowGeneralPanel()
    {
        HideAllPanels();
        generalPanel.SetActive(true);

        title.text = "General";

        SetMenuVisible(false);
    }

    public void ShowStartupPanel()
    {
        HideAllPanels();
        startupPanel.SetActive(true);

        title.text = "Startup";

        SetMenuVisible(false);
    }

    public void ShowAddressablesPanel()
    {
        HideAllPanels();
        addressablesPanel.SetActive(true);

        title.text = "Addressables";

        SetMenuVisible(false);
    }

    public void ShowConsolePanel()
    {
        HideAllPanels();
        consolePanel.SetActive(true);

        title.text = "Console";

        SetMenuVisible(false);
    }
}
