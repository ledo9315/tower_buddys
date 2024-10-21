using UnityEngine;

public class Shop : MonoBehaviour
{
    private BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.Instance;
    }

    public void PurchaseStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
    }

    public void PurchasedAnotherTurret()
    {
        Debug.Log("Another Turret Selected");
        buildManager.SetTurretToBuild((buildManager.anotherTurretPrefab));

    }
}
