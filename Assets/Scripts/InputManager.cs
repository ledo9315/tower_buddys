using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;


public class InputManager : MonoBehaviour
{
    NodeManager nodeManager;
    Shop shopManager;
    //Da wir mit den Movement Buttons auch durch Menüs usw. navigieren müssen
    //sollten wir später wie auch bei SpieleProg über ein Enum ermitteln
    //in welchem State das Spiel ist. Da wir kein Menü oder w/e
    //haben, habe ich diesen Wert konstant auf _inRunningGame gesetzt.
    bool _inRunningGame = true;

    void Start()
    {
        nodeManager = FindAnyObjectByType<NodeManager>().GetComponent<NodeManager>();
        shopManager = FindAnyObjectByType<Shop>().GetComponent<Shop>();
    }

    public void UseOfMovementButtons(CallbackContext cb) {
        Vector2 mvValFloat = cb.ReadValue<Vector2>();

        //Wird eine Taste gedrückt, dann wird diese Methode hier mehrere Male aufgerufen. Hier wird returned, wenn der Knopf nicht gerade
        //gedrückt wurde https://discussions.unity.com/t/player-input-component-triggering-events-multiple-times/781922
        if (!cb.started) return;


        //Wert wird auf Int gerundet, um evtl. Joystick-Probleme zu umgehen.
        //int xVal = (int)Math.Round(mvValFloat.x);
        //int yVal = (int)Math.Round(mvValFloat.y);
        int xVal = 0;
        int yVal = 0;

        if (Mathf.Abs(mvValFloat.x) > 0.05) xVal = mvValFloat.x > 0 ? (int)Math.Ceiling(mvValFloat.x) : (int)Math.Floor(mvValFloat.x);
        if (Mathf.Abs(mvValFloat.y) > 0.05) yVal = mvValFloat.y > 0 ? (int)Math.Ceiling(mvValFloat.y) : (int)Math.Floor(mvValFloat.y);

        Vector2Int mvValInt = new Vector2Int(xVal, yVal);

        Debug.Log("Float x: " + mvValFloat.x + " Int x: " + xVal);
        Debug.Log("Float Y: " + mvValFloat.y + " Int y: " + yVal);

        //Läuft das Game und existiert der Nodemanager, so wird die neue Node ausgewählt
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

        if (_inRunningGame && nodeManager != null) shopManager.SelectNextTurret();
    }
}
