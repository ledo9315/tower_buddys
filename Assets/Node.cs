using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    [SerializeField] private Vector3 possitionOffset;
    private GameObject _turret;
    private Renderer _rend;
    private Color _startColor;

    private BuildManager buildManager;
    
    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _startColor = _rend.material.color;
        buildManager = BuildManager.Instance;
    }

    private void OnMouseDown()
    {
        if(buildManager.GetTurretToBuild() == null) return;
        
        if (_turret != null)
        {
            Debug.Log("Can't build there");
            return;
        }

        GameObject turretToBuild = BuildManager.Instance.GetTurretToBuild();
        _turret = Instantiate(turretToBuild, transform.position + possitionOffset, transform.rotation);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if(buildManager.GetTurretToBuild() == null) return;

        _rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        _rend.material.color = _startColor;
    }
}
