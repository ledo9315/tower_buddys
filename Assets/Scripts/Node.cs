using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Node : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notEnoughMoneyColor;
    [SerializeField] private Vector3 positionOffset;
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

    public void NodeIsSelected() {
        _rend.material.color = buildManager.HasMoney ? hoverColor : notEnoughMoneyColor;
    }

    public void NodeIsNotSelected() {
        _rend.material.color = _startColor;
    }

    public void TryBuilding()
    {
        if (!buildManager.CanBuild) return; //Ist schon ein Turm ausgewählt?
        if (turret != null) return;         //Ist schon ein Turm gebaut?

        buildManager.BuildTurretOn(this);   //Alle Checks durch, baue Turm
    }
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    /*
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
    } */
}
