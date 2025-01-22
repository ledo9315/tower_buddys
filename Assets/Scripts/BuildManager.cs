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

    public GameObject BuildTurretOn(Node node)
    {

        //TODO Bitte lass uns das ändern. Warum ist die Logik zum Überprüfen, ob man genug Geld hat
        //Nicht im Script, welches sich ums Geld kümmern sollte.
        if (PlayerStats.Money < turretToBuild.cost)
        {
            Debug.Log("Not enough money");
            return null;
        }

        PlayerStats.Money -= turretToBuild.cost;
        
        GameObject turret = Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);

        Debug.Log("Turret build! Money left: " + PlayerStats.Money);
        return turret;
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }
}
