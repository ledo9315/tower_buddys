using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] int xNodes;
    [SerializeField] int zNodes;
    Node[,] nodeArr;
    Vector2Int currSelected = new(7, 7);
    private float newMoneytimer = 2.5f;
    private float currentMoneyTimer = 0;
    
    void Start()
    {
        nodeArr = new Node[xNodes, zNodes];

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
        
        nodeArr[currSelected.x, currSelected.y].NodeIsSelected();
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
        while (yPos >= 0 && yPos < zNodes && !yPosFound)
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
    public void ActionOnSelectedNode()
    {
        nodeArr[currSelected.x, currSelected.y].TryAction();
    }

    private void Update()
    {
        currentMoneyTimer += Time.deltaTime;
        if (currentMoneyTimer >= newMoneytimer)
        {
            currentMoneyTimer = 0;
            PlaceMoneyNode();
        }
    }

    //Sucht eine freie Node und platziert hier eine Money-Node
    public void PlaceMoneyNode()
    {
        Vector2Int randPos = new(UnityEngine.Random.Range(0, xNodes), UnityEngine.Random.Range(0, zNodes));

        Vector2Int newPos = FindClosestNodeWithProperty(randPos, NodeMode.IsEmpty);
        if (newPos.x >= 0 || newPos.y >= 0) {
            nodeArr[newPos.x, newPos.y].NodeIsMoney();
        }
    }

    //Eventuell Funktion hier rausl�schen, da FindClosestNodeWithProperty(), wenn keine Node gefunden wird -1 -1 zur�ckgibt.
    private bool DoesNodeWithPropertyExist(NodeMode propertyOfNeededNode) {
        for (int xCounter = 0; xCounter < xNodes; xCounter++)        {
            for (int yCounter = 0; yCounter < zNodes; yCounter++)            {
                if (nodeArr[xCounter, yCounter] != null) {
                    if (propertyOfNeededNode == nodeArr[xCounter, yCounter].GetCurrMode()) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //TODO Effizienter Machen, das ist super ineffizient
    private Vector2Int FindClosestNodeWithProperty(Vector2Int startingNode, NodeMode propertyOfNeededNode)    {
        Vector2Int closestNode = new(-1, -1); // Initialize with invalid coordinates
        float minDistance = 10000;

        for (int xCounter = 0; xCounter < xNodes; xCounter++){
            for (int yCounter = 0; yCounter < zNodes; yCounter++){
                if (nodeArr[xCounter, yCounter] != null && propertyOfNeededNode == nodeArr[xCounter, yCounter].GetCurrMode()){

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
