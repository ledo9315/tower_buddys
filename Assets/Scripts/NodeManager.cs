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

    //NodeMode ist auch in Node.cs. Wenn das hier geändert wird, muss Node.cs angepasst werden!
    //Mir fällt keine gute Art und Weise ein, NodeMode nur im NodeManager zu haben.
    //TODO: Irgendwann umschreiben, sodass Node.cs diesen Enum nicht hält
    private enum NodeMode { IsEmpty = 0, IsSelect = 1, IsBuilt = 2, NotEnoughMoney = 3, IsMoney = 4, IsAmmunition = 5, IsBonus = 6 };

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
        while (xPos >= 0 && xPos < xNodes && !xPosFound)
        {
            if (nodeArr[xPos, currSelected.y] == null)
            {
                xPos += dir.x;
            }
            else
            {
                nodeArr[currSelected.x, currSelected.y].NodeIsNotSelected();
                currSelected.x = xPos;
                xPosFound = true;
            }
        }

        bool yPosFound = false;
        while (yPos >= 0 && yPos < xNodes && !yPosFound)
        {
            if (nodeArr[currSelected.x, yPos] == null)
            {
                yPos += dir.y;
            }
            else
            {
                nodeArr[currSelected.x, currSelected.y].NodeIsNotSelected();
                currSelected.y = yPos;
                yPosFound = true;
            }
        }

        nodeArr[currSelected.x, currSelected.y].NodeIsSelected();
    }

    //Baut auf SelectedNode
    public void BuildOnSelectedNode()
    {
        nodeArr[currSelected.x, currSelected.y].TryBuilding();
    }

    private bool DoesNodeWithPropertyExist(NodeMode propertyOfNeededNode) {
        for (int xCounter = 0; xCounter < xNodes; xCounter++)        {
            for (int yCounter = 0; yCounter < yNodes; yCounter++)            {
                var currentNode = nodeArr[xCounter, yCounter];

                if (currentNode != null) {
                    if ((int)propertyOfNeededNode == currentNode.GetCurrMode()) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private Vector2Int FindClosestNodeWithProperty(Vector2Int startingNode, NodeMode propertyOfNeededNode)
    {
        Vector2Int closestNode = new Vector2Int(-1, -1); // Initialize with invalid coordinates
        float minDistance = 10000;

        for (int xCounter = 0; xCounter < xNodes; xCounter++){
            for (int yCounter = 0; yCounter < yNodes; yCounter++){
                var currentNode = nodeArr[xCounter, yCounter];

                if (currentNode != null && (int)propertyOfNeededNode == currentNode.GetCurrMode()){
                    // Calculate Euclidean distance
                    float distance = Mathf.Sqrt(Mathf.Pow(xCounter - startingNode.x, 2) + Mathf.Pow(yCounter - startingNode.y, 2));

                    // Update closest node if a smaller distance is found
                    if (distance < minDistance){
                        minDistance = distance;
                        closestNode = new Vector2Int(xCounter, yCounter);
                    }
                }
            }
        }

        return closestNode; // Return the closest node's coordinates
    }
}
