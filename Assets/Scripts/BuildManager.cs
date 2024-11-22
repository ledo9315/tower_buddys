using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    [SerializeField] private GameObject standardTurretPrefab;
    [SerializeField] private GameObject missileLauncherPrefab;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this; 
    }


    
    private TurretBlueprint turretToBuild;
    
    public bool CanBuild => turretToBuild != null;
    public bool HasMoney => PlayerStats.Money >= turretToBuild.cost;

    public bool BuildTurretOn(Node node)
    {
        if (PlayerStats.Money < turretToBuild.cost)
        {
            Debug.Log("Not enough money");
            return false;
        }

        PlayerStats.Money -= turretToBuild.cost;
        
        GameObject turret = Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        Debug.Log("Turret build! Money left: " + PlayerStats.Money);
        return true;
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }
}
