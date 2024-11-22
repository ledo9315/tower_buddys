using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Node : MonoBehaviour
{
    [Header("Colours")]
    [SerializeField] private Color selectColor;
    [SerializeField] private Color notSelectColor;
    [SerializeField] private Color notEnoughMoneyColor;
    [SerializeField] private Color isMoneyColor;
    [SerializeField] private Color isAmmunitionColor;
    [SerializeField] private Color isBonusColor;

    [Header("Optional")]
    [SerializeField] private Vector3 positionOffset;
    [FormerlySerializedAs("_turret")] public GameObject turret;

    private Renderer _rend;
    private enum NodeMode { IsEmpty, IsSelect, IsBuilt, NotEnoughMoney, IsMoney, IsAmmunition, IsBonus };
    private NodeMode _mode = NodeMode.IsEmpty;
    private BuildManager buildManager;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _rend.material.color = notSelectColor;
        buildManager = BuildManager.Instance;
    }

    public void NodeIsSelected()
    {
        _mode = NodeMode.IsSelect;
        UpdateNodeColor();
    }

    //TODO: Entscheiden, ob es IsEmpty oder IsNotSelected heißen soll!
    public void NodeIsNotSelected()
    {
        _mode = NodeMode.IsEmpty;
        UpdateNodeColor();
    }

    private void UpdateNodeColor()
    {
        //Schaut sich den derzeitigen Modus an und entscheidet dann
        //Hier können dann auch andere Funktionen aufgerufen werden
        //z.B. falls Felder umrandet sein sollen, wenn die Selected sind,
        //kann evtl. neues Objekt Instantiiert werden

        switch (_mode)
        {
            //Wenn das Feld leer ist
            case NodeMode.IsEmpty:
                _rend.material.color = notSelectColor;
                break;

            //Das Feld wurde ausgewählt
            case NodeMode.IsSelect:
                _rend.material.color = selectColor;
                break;

            //Das Feld ist bebaut
            case NodeMode.IsBuilt:
                _rend.material.color = notSelectColor;
                break;

            //Du hattest nicht genug Geld
            case NodeMode.NotEnoughMoney:
                _rend.material.color = notEnoughMoneyColor;
                break;

            //Das Feld ist ein Geld-Feld
            case NodeMode.IsMoney:
                _rend.material.color = isMoneyColor;
                break;

            //Das Feld ist ein Munitions-Feld
            case NodeMode.IsAmmunition:
                _rend.material.color = isAmmunitionColor;
                break;

            //Das Feld ist ein Bonus-Feld
            case NodeMode.IsBonus:
                _rend.material.color = isBonusColor;
                break;

            //Irgendwas ist schief gelaufen!
            default:
                _rend.material.color = notSelectColor;
                Debug.Log("Node.CS hat einen Fehler, hier solltest du nicht landen!");
                break;
        }
    }

    public void TryBuilding()
    {
        if (!buildManager.CanBuild) return; //Ist schon ein Turm ausgewählt?
        if (turret != null) return;         //Ist schon ein Turm gebaut?

        bool wasAbleToBuild = buildManager.BuildTurretOn(this);   //Alle Checks durch, baue Turm
        if (wasAbleToBuild)
        {
            _mode = NodeMode.IsBuilt;
            UpdateNodeColor();
        }
    }
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }
}