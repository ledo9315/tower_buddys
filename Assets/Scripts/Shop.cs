using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Button standardTurrButton;
    [SerializeField] Button missileTurrButton;
    [SerializeField] Color selectedColor;
    [SerializeField] Color notSelectedColor;
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    private BuildManager buildManager;
    private int _currentShopItem = 0;
    private int _amountOfShopItems = 2;
    
    private void Start()
    {
        buildManager = BuildManager.Instance;
        SelectStandardTurret();
    }

    public void SelectNextTurret()
    {
        Debug.Log(_currentShopItem);
        _currentShopItem = (_currentShopItem + 1) % _amountOfShopItems;
        Debug.Log(_currentShopItem);
        switch (_currentShopItem)
        {
            case 0:
                SelectStandardTurret();
                break;
            case 1:
                SelectMissileLauncher();
                break;
        }
    }

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);

        // Update the color for the standard turret button
        ColorBlock cb = standardTurrButton.colors;
        cb.normalColor = selectedColor;
        standardTurrButton.colors = cb;

        // Update the color for the missile turret button
        ColorBlock cb2 = missileTurrButton.colors;
        cb2.normalColor = notSelectedColor;
        missileTurrButton.colors = cb2;

        // Force the buttons to refresh their visual state
        standardTurrButton.OnDeselect(null);
        missileTurrButton.OnDeselect(null);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        buildManager.SelectTurretToBuild(missileLauncher);

        // Update the color for the missile turret button
        ColorBlock cb = missileTurrButton.colors;
        cb.normalColor = selectedColor;
        missileTurrButton.colors = cb;

        // Update the color for the standard turret button
        ColorBlock cb2 = standardTurrButton.colors;
        cb2.normalColor = notSelectedColor;
        standardTurrButton.colors = cb2;

        // Force the buttons to refresh their visual state
        missileTurrButton.OnDeselect(null);
        standardTurrButton.OnDeselect(null);
    }
}
