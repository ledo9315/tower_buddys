using System;
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

    [Header("Button Buyscreen")]
    [SerializeField] Button microwaveTurrButton;
    [SerializeField] Button fridgeTurrButton;
    [SerializeField] Button ovenTurrButton;
    
    [Header("Buy Price Texts")]
    [SerializeField] private TextMeshProUGUI[] buyTextArray;

    [Header("Button Upgradescreen")]
    [SerializeField] Button upgradeLeft;
    [SerializeField] Button upgradeMid;
    [SerializeField] Button upgradeRight;
    
    [Header("Upgrade Price Texts")]
    [SerializeField] private TextMeshProUGUI[] upgradeTextArray;

    [Header("Colours")]
    [SerializeField] Color selectedColor;
    [SerializeField] Color notSelectedColor;
    
    [Header("Turret Blueprints")]
    [SerializeField] private TurretBlueprint microwaveTurret;
    [SerializeField] private TurretBlueprint fridgeTurret;
    [SerializeField] private TurretBlueprint ovenTurret;
    
    private Turret turrScript;
    private BuildManager buildManager;
    
    private int _currentBuyItem = 0;
    private int _currentUpgradeItem = 3;
    private int _amountOfShopItems = 3;

    private float currComboTime = 0;
    public static int currentSelectedUpgrade = 0;
    
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
        UpdateBuyScreen();
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

    public void ChangeToUpgradeScreen(Turret newTurrScript)
    {
        turrScript = newTurrScript;
        _currScreenMode = ScreenMode.UpgradeScreen;
        UpdateUIConfig();
    }

    private void UpdateUIConfig() {
        switch (_currScreenMode) {
            case ScreenMode.BuyScreen:
                UpdateBuyScreen();
                BuyScreen.SetActive(true);
                UpgradeScreen.SetActive(false);
                break;
            case ScreenMode.UpgradeScreen:
                UpdateUpgradeScreen();
                BuyScreen.SetActive(false);
                UpgradeScreen.SetActive(true);
                break;
        }

        UpdateValues();
    }

    public void SelectNext() {
        switch (_currScreenMode) {
            case ScreenMode.BuyScreen:
                _currentBuyItem++;
                UpdateBuyScreen();
                break;
            case ScreenMode.UpgradeScreen:
                _currentUpgradeItem++;
                currentSelectedUpgrade = _currentUpgradeItem;
                UpdateUpgradeScreen();
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

    private void UpdateBuyScreen()
    {
        ColorBlock cb = microwaveTurrButton.colors;
        cb.normalColor = notSelectedColor;
        microwaveTurrButton.colors = cb;
        fridgeTurrButton.colors = cb;
        ovenTurrButton.colors = cb;
        
        cb.normalColor = selectedColor;
        switch (_currentBuyItem % 3)
        {
            case 0:
                microwaveTurrButton.colors = cb;
                buildManager.SelectTurretToBuild(microwaveTurret);
                Debug.Log("microwaveTurrButton");
                break;
            case 1:
                fridgeTurrButton.colors = cb;
                buildManager.SelectTurretToBuild(fridgeTurret);
                Debug.Log("fridgeTurrButton");
                break;
            case 2:
                ovenTurrButton.colors = cb;
                buildManager.SelectTurretToBuild(ovenTurret);
                Debug.Log("ovenTurrButton");
                break;
        }
    }
    
    private void UpdateUpgradeScreen()
    {
        ColorBlock cb = microwaveTurrButton.colors;
        cb.normalColor = notSelectedColor;
        upgradeLeft.colors = cb;
        upgradeMid.colors = cb;
        upgradeRight.colors = cb;
        
        cb.normalColor = selectedColor;
        switch (_currentUpgradeItem % 3)
        {
            case 0:
                upgradeLeft.colors = cb;
                break;
            case 1:
                upgradeMid.colors = cb;
                break;
            case 2:
                upgradeRight.colors = cb;
                break;
        }

    }

    public void UpdateValues()
    {
        switch (_currScreenMode) {
            case ScreenMode.BuyScreen:
                buyTextArray[0].text = microwaveTurret.cost.ToString() + " $";
                buyTextArray[1].text = fridgeTurret.cost.ToString() + " $";
                buyTextArray[2].text = ovenTurret.cost.ToString() + " $";
                break;
            case ScreenMode.UpgradeScreen:
                for (int element = 0; element < upgradeTextArray.Length; element++)
                {
                    upgradeTextArray[element].text = turrScript.upgradePrice[element].ToString() + " $";
                }
                break;
        }
    }
}
