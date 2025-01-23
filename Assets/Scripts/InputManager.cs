using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class InputManager : MonoBehaviour
{
    NodeManager nodeManager;
    //Da wir mit den Movement Buttons auch durch Men�s usw. navigieren m�ssen
    //sollten wir sp�ter wie auch bei SpieleProg �ber ein Enum ermitteln
    //in welchem State das Spiel ist. Da wir kein Men� oder w/e
    //haben, habe ich diesen Wert konstant auf _inRunningGame gesetzt.
    bool _inRunningGame = true;
    bool _tryToTeleport = false;
    public InputActionReference teleportAction;
    public XROrigin playerOrigin;
    void Start()
    {
        nodeManager = FindAnyObjectByType<NodeManager>().GetComponent<NodeManager>();
        playerOrigin = FindAnyObjectByType<XROrigin>().GetComponent<XROrigin>();
    }

    private void Awake()
    {
        teleportAction.action.Enable();
        teleportAction.action.performed += UseOfGripButton;
        teleportAction.action.canceled += UseOfGripButton;

    }
    
    private void OnDestroy()
    {
        teleportAction.action.Disable();
        teleportAction.action.performed -= UseOfGripButton;
        teleportAction.action.canceled -= UseOfGripButton;

    }

    public void UseOfMovementButtons(CallbackContext cb) {
        Vector2 mvValFloat = cb.ReadValue<Vector2>();

        //Wird eine Taste gedr�ckt, dann wird diese Methode hier mehrere Male aufgerufen. Hier wird returned, wenn der Knopf nicht gerade
        //gedr�ckt wurde https://discussions.unity.com/t/player-input-component-triggering-events-multiple-times/781922
        if (!cb.started) return;


        //Wert wird auf Int gerundet, um evtl. Joystick-Probleme zu umgehen.
        //int xVal = (int)Math.Round(mvValFloat.x);
        //int yVal = (int)Math.Round(mvValFloat.y);
        int xVal = 0;
        int yVal = 0;

        if (Mathf.Abs(mvValFloat.x) > 0.05) xVal = mvValFloat.x > 0 ? (int)Math.Ceiling(mvValFloat.x) : (int)Math.Floor(mvValFloat.x);
        if (Mathf.Abs(mvValFloat.y) > 0.05) yVal = mvValFloat.y > 0 ? (int)Math.Ceiling(mvValFloat.y) : (int)Math.Floor(mvValFloat.y);

        Vector2Int mvValInt = new Vector2Int(xVal, yVal);

        //Debug.Log("Float x: " + mvValFloat.x + " Int x: " + xVal);
        //Debug.Log("Float Y: " + mvValFloat.y + " Int y: " + yVal);

        //L�uft das Game und existiert der Nodemanager, so wird die neue Node ausgew�hlt
        if (_inRunningGame && nodeManager != null) nodeManager.SelectNewNode(mvValInt);
    }

    public void UseOfActionButton(CallbackContext cb) {
        float actVal = cb.ReadValue<float>();

        if (!cb.started) return;//Siehe Doku bei UseOfMovement

        if (_inRunningGame && nodeManager != null) nodeManager.BuildOnSelectedNode();
    }

    public void UseOfOptionButton(CallbackContext cb)
    {
        float optVal = cb.ReadValue<float>();

        if (!cb.started) return;

        if (_inRunningGame && nodeManager != null) Shop.Instance.SelectNext();
    }

    public void UseOfGripButton(CallbackContext cb)
    {
        GameObject ctrl = GameObject.FindGameObjectWithTag("rightController");
        //Debug.Log("UseOfGripButton");
        if (!cb.started) return;
        if (_tryToTeleport)
        {
            ctrl.GetComponent<RaycastExample>().activateTeleporter();
        }
        else
        {
            ctrl.GetComponent<RaycastExample>().deactivateTeleporter();
            Vector3 newPos = ctrl.GetComponent<RaycastExample>().getRaycastHitLocation();
            newPos.y = 2.11f;
            playerOrigin.GetComponent<Transform>().position = newPos;
            
        }
        _tryToTeleport = !_tryToTeleport;
    }
}
