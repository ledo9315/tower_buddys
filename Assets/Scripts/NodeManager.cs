using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] int xNodes;
    [SerializeField] int yNodes;
    Node[,] nodeArr;
    Vector2Int currSelected = new Vector2Int(2, 2);

    void Start()
    {
        nodeArr = new Node[xNodes, yNodes];

        GameObject[] tempNodeArr = GameObject.FindGameObjectsWithTag("Node");
        Vector3[] tempPositionArr = new Vector3[tempNodeArr.Length];

        for (int i = 0; i < tempNodeArr.Length; i++)
        {
            tempPositionArr[i] = tempNodeArr[i].transform.position;
        }

        for (int i = 0; i < tempPositionArr.Length; i++)
        {
            int xPos = (int)(tempPositionArr[i].x / 5f);
            int yPos = (int)(tempPositionArr[i].z / 5f);
            nodeArr[xPos, yPos] = tempNodeArr[i].GetComponent<Node>();
        }
    }

    public void SelectNewNode(Vector2Int dir)
    {

        int xPos = dir.x + currSelected.x;
        int yPos = dir.y + currSelected.y;

        //Solange das neue Feld auf dem Spielfeld passt und noch kein valides Feld gefunden wurde:
        //Check, ob das Feld null ist
        //Wenn ja, gehe noch ein Feld weiter
        //Wenn nein, du hast dein Feld gefunden
        bool xPosFound = false;
        while (xPos >= 0 && xPos < xNodes && !xPosFound){
            if (nodeArr[xPos, currSelected.y] == null){
                xPos += dir.x;
            }else{
                nodeArr[currSelected.x, currSelected.y].NodeIsNotSelected();
                currSelected.x = xPos;
                xPosFound = true;
            }
        }

        bool yPosFound = false;
        while (yPos >= 0 && yPos < xNodes && !yPosFound)
        {
            if (nodeArr[currSelected.x, yPos] == null){
                yPos += dir.y;
            }else{
                nodeArr[currSelected.x, currSelected.y].NodeIsNotSelected();
                currSelected.y = yPos;
                yPosFound = true;
            }
        }

        nodeArr[currSelected.x, currSelected.y].NodeIsSelected();
    }

    public void BuildOnSelectedNode() {
        nodeArr[currSelected.x, currSelected.y].TryBuilding();
    }

}
