using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Node : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notEnoughMoneyColor;
    [SerializeField] private Vector3 possitionOffset;
    [Header("Optional")]
    [FormerlySerializedAs("_turret")] public GameObject turret;
    private Renderer _rend;
    private Color _startColor;

    private BuildManager buildManager;
    
    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _startColor = _rend.material.color;
        buildManager = BuildManager.Instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + possitionOffset;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if(!buildManager.CanBuild) return;
        
        if (turret != null)
        {
            Debug.Log("Can't build there");
            return;
        }

        buildManager.BuildTurretOn(this);
        
        // GameObject turretsParent = GameObject.Find("Turrets");
        // if (turretsParent != null)
        // {
        //     turret.transform.SetParent(turretsParent.transform);
        // }
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if(!buildManager.CanBuild) return;
        _rend.material.color = buildManager.HasMoney ? hoverColor : notEnoughMoneyColor;
    }

    private void OnMouseExit()
    {
        _rend.material.color = _startColor;
    }
}
