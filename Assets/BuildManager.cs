using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this; 
    }

    public GameObject standardTurretPrefab;
    public GameObject anotherTurretPrefab;
    
    private GameObject turretToBuild;

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;
    }
}
