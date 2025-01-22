using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject BuyScreen;
    [SerializeField] private GameObject UpgradeScreen;

    [Header("Selection Texts")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI powerupText;
    [SerializeField] private TextMeshProUGUI comboTimerText;

    [Header("Colours")]
    [SerializeField] Button standardTurrButton;
    [SerializeField] Color selectedColor;
    [SerializeField] Color notSelectedColor;

    public TurretBlueprint standardTurret;
    private BuildManager buildManager;
    private int _currentBuyItem = 0;
    private int _currentUpgradeItem = 0;
    private int _amountOfShopItems = 2;

    private float currComboTime = 0;

    private enum ScreenMode
    {
        BuyScreen = 0,
        UpgradeScreen = 1,
    }
    private ScreenMode _currScreenMode;

    public static Shop Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        WaveSpawner.OnNewWaveLoaded += UpdateWaveText;
        PlayerStats.OnHealthChanged += UpdateHealthText;
    }

    private void OnDisable()
    {
        WaveSpawner.OnNewWaveLoaded -= UpdateWaveText;
        PlayerStats.OnHealthChanged -= UpdateHealthText;
    }

    private void Start()
    {
        buildManager = BuildManager.Instance;
        _currScreenMode = ScreenMode.BuyScreen;
        SelectStandardTurret();
    }
    void Update()
    {
        currComboTime = PlayerStats.comboTimer;
        comboTimerText.text = "Combo: " + currComboTime.ToString("F1");
    }

    public void ChangeToBuyScreen() {
        _currScreenMode = ScreenMode.BuyScreen;
        UpdateUIConfig();
    }

    public void ChangeToUpgradeScreen(GameObject turretGameObject) {
        _currScreenMode = ScreenMode.UpgradeScreen;
        UpdateUIConfig();
        //Hiest mit dem Turret die Upgradetexte befüllen
    }

    private void UpdateUIConfig() {
        switch (_currScreenMode) {
            case ScreenMode.BuyScreen:
                BuyScreen.SetActive(true);
                UpgradeScreen.SetActive(false);
                break;
            case ScreenMode.UpgradeScreen:
                BuyScreen.SetActive(false);
                UpgradeScreen.SetActive(true);
                break;
        }
    }

    public void SelectNext() {
        switch (_currScreenMode) {
            case ScreenMode.BuyScreen:
                _currentBuyItem++;
                break;
            case ScreenMode.UpgradeScreen:
                _currentUpgradeItem++;
                break;
        }
    }

    private void UpdateWaveText(int currWave)
    {
        waveText.text = "Wave: " + currWave.ToString();
    }

    private void UpdateHealthText(int currHealth)
    {
        healthText.text = "Lives: " + currHealth.ToString();
    }

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);

        // Update the color for the standard turret button
        ColorBlock cb = standardTurrButton.colors;
        cb.normalColor = selectedColor;
        standardTurrButton.colors = cb;

        // Force the buttons to refresh their visual state
        standardTurrButton.OnDeselect(null);
    }
}
