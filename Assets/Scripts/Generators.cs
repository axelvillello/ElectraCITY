using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Generators : MonoBehaviour
{
    List<Node> connectionTree;
    Queue<GameObject> queue;
    [SerializeField] int powerGen;
    int totalConnectionsCost;
    int blackWireTotal, redWireTotal, yellowWireTotal;
    bool tutorialObjectStatus = false;

    private void Start()
    {
        queue = new Queue<GameObject>();
        transform.GetComponent<ConnectorGen>().GenerateConnectors();
        Plant();
    }


    public void Plant()
    {
        connectionTree = new List<Node>();
        foreach (Transform child in transform)
        {
            WireConnection wc = child.gameObject.GetComponent<WireConnection>();
            if (wc.otherConnector != null)
            {
                connectionTree.Add(new Node(wc.otherConnector.GetComponent<WireConnection>().getParent(), gameObject, child.GetComponent<WireConnection>().getResistance()));
                queue.Enqueue(wc.otherConnector.GetComponent<WireConnection>().getParent());
                wc.otherConnector.GetComponent<WireConnection>().getParent().GetComponent<Consumers>().setInGen(true);
            }
        }
        connectionTree = Tree(10, connectionTree, queue);
        PowerLine();
    }

    //Generates a list of consumers related to the closest generators
    private List<Node> Tree(int depth, List<Node> tree, Queue<GameObject> queue)
    {
        int connections = 0;
        GameObject currentObject;
        while (depth > 0 && queue.Count > 0)
        {
            currentObject = queue.Dequeue();
            if (currentObject.tag == "Generator")
            {
                //No need to continue this line
            }
            else if (currentObject.tag == "Consumer")
            {
                //currentObject.gameObject.GetComponent<Consumers>().setInGen(true);
                //How many used connections in the object
                Node[] toSort = new Node[currentObject.transform.childCount];
                for (int i = 3; i < currentObject.transform.childCount; i++)
                {
                    GameObject otherConnector = currentObject.transform.GetChild(i).gameObject.GetComponent<WireConnection>().otherConnector;
                    if (otherConnector != null)
                    {
                        WireConnection otherConnection = otherConnector.GetComponent<WireConnection>();
                        if (otherConnection.getParent().tag == "Consumer" && !otherConnection.getParent().GetComponent<Consumers>().getInGen())
                        {
                            connections++;
                            //Debug.Log(otherConnection.getParent() + " from " + currentObject + " at resistance " + otherConnection.getResistance());
                            toSort[i] = new Node(otherConnection.getParent(), currentObject, otherConnection.getResistance());
                            tree.Add(new Node(otherConnection.getParent(), currentObject, otherConnection.getResistance()));
                            queue.Enqueue(otherConnection.getParent());
                            otherConnection.getParent().GetComponent<Consumers>().setInGen(true);
                        }
                    }
                }
                while (toSort.Length <= 0)
                {
                    int lowest = 0;
                    for (int i = 0; i < toSort.Length; i++)
                    {
                        if (toSort[i].getResistance() < toSort[lowest].getResistance())
                        {
                            lowest = i;
                        }
                    }
                    tree.Add(toSort[lowest]);
                    toSort[lowest] = null;
                }
            }
            depth--;
        }
        queue.Clear();
        totalConnectionsCost = 0;
        blackWireTotal = 0;
        redWireTotal = 0;
        yellowWireTotal = 0;
        for (int i = 0; i < tree.Count; i++)
        {
            Debug.Log(tree[i].getSelf() + " from " + tree[i].getOrigin() + " at resistance " + tree[i].getResistance());

            float distanceFactor = Mathf.Clamp01(tree[i].getDistance() / 3000f);
            int scaledCost = Mathf.RoundToInt(tree[i].getCost() * distanceFactor);

            if (scaledCost == 0)
            {
                scaledCost = 1;
            }
            
            totalConnectionsCost += scaledCost;


            switch (tree[i].getResistance())
            {
                case 3:
                    blackWireTotal++;
                    break;
                case 1:
                    redWireTotal++;
                    break;
                case 0:
                    yellowWireTotal++;
                    break;
            }
        }
        return tree;
    }

    private void PowerLine()
    {
        foreach (Node item in connectionTree)
        {
            Node currentItem = item;
            if (item.getPowerPercent() >= 100) { continue; } //Skip if already on

            item.setCurrentPower(powerGen - item.getResistance()); //Set amount given by max - self resistance

            while (currentItem.getOrigin().tag != "Generator") //While origin is not a generator
            {
                foreach (Node next in connectionTree)
                {
                    if (GameObject.ReferenceEquals(next.getSelf(), currentItem.getOrigin())) //Find object in node list that is origin
                    {
                        currentItem = next;
                        break;
                    }
                }
                item.setCurrentPower(item.getCurrentPower() - currentItem.getResistance());
            }
            if (item.getCurrentPower() >= 0)
            {
                item.addPowerPercent(100);
            }
            else
            {
                if (item.getCurrentPower() + item.getResistance() > 0)
                {
                    item.addPowerPercent(((item.getCurrentPower() + item.getResistance()) * 100) / item.getResistance());
                }
            }
        }
    }

    public int getTotalConnectionsCost()
    {
        return totalConnectionsCost;
    }

    public (int black, int red, int yellow) getTotalWires()
    {
        return (blackWireTotal, redWireTotal, yellowWireTotal);
    }

    public bool setTutorialObjectStatus(bool status)
    {
        tutorialObjectStatus = status;

        return tutorialObjectStatus;
    }
}
