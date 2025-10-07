using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WireConnection : MonoBehaviour
{
    private Global global;
    private int resistance;
    private int wireType;
    private GameObject parent;
    private int maxDelta = 1000;
    private System.Random random = new System.Random();
    public Boolean selected;
    public Boolean isDragging = false;
    public LineRenderer lineRenderer;
    public GameObject otherConnector;
    public int length = 0;

    private void Awake()
    {
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>();

        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        //lineRenderer.sortingLayerID = 0;
        //lineRenderer.sortingOrder = 3;
        parent = this.transform.parent.gameObject;
    }
    void Update()
    {
        if (selected)
        {
            global.wireSelected = true;
            Vector3 startPos = lineRenderer.GetPosition(0);
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(1, Vector3.MoveTowards(startPos, endPos, maxDelta));

            length = 0;

            if (global.connector == null)
            {
                global.wireSelected = false;
                selected = false;
                lineRenderer.SetPosition(1, transform.position);
                lineRenderer.Simplify(1f);

            }
        }
        if (otherConnector)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, otherConnector.transform.position);
            otherConnector.GetComponent<Renderer>().material.SetColor("_Color", lineRenderer.startColor);

            //CAUSES: null reference exception
            //otherConnector.GetComponent<WireConnection>().length = (int)(global.connector.transform.position - transform.position).magnitude; 
        }        
    }

    private void OnMouseDown()
    {
        if (global.wireID != 0)
        {
            global.Play("Click" + random.Next(1, 4).ToString());            
        }
        //If object has other connector it is wired
        //If global connector is not null, wire is being dragged
        if (!otherConnector)
        {
            wireType = global.wireID;            
        }   
        switch (wireType) //Choose color based on wire selected
        {
            case 0:
                //Debug.Log("None");
                break;
            case 1:
                //Debug.Log("Black");
                lineRenderer.startColor = new Color(58f / 255f, 129f / 255f, 255f / 255f);
                lineRenderer.endColor = new Color(58f / 255f, 129f / 255f, 255f / 255f);
                maxDelta = 10000;
                break;
            case 2:
                //Debug.Log("Red");
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
                maxDelta = 10000;
                break;
            case 3:
                //Debug.Log("Yellow");
                lineRenderer.startColor = Color.yellow;
                lineRenderer.endColor = Color.yellow;
                maxDelta = 10000;
                break;
        }
        if (otherConnector && global.connector == null)
        {
            //Move Connection
            /////////////////////
            //Remove line to other connector
            lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
            lineRenderer.Simplify(1f);
            //Other Connector Selection
            WireConnection otherWire = otherConnector.GetComponent<WireConnection>();
            otherWire.selected = true;
            global.connector = otherConnector;
            global.connector.GetComponent<Renderer>().material.SetColor("_Color", lineRenderer.startColor);
            //Delete Previous
            otherWire.otherConnector.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            global.wireOhm = otherWire.resistance;
            otherWire.otherConnector = null;
            otherConnector = null;
            //Wire Type change
            global.wireID = wireType;

            length = (int)(global.connector.transform.position - transform.position).magnitude;


        }
        else if (otherConnector && global.connector != null)
        {
            //Space for incorrect selection effects

            length = 0;

            return;
        }
        else if (global.wireID != 0)
        {
            if (global.connector != null) //If Another Connector is Selected
            {
                if ((global.connector.transform.position - transform.position).magnitude > maxDelta)
                {
                    Debug.Log("Wire too Long");
                    Debug.Log((global.connector.transform.position - transform.position).magnitude);
                    return;
                }
                bool same = false;
                foreach (Transform item in global.connector.GetComponent<WireConnection>().transform.parent.transform)
                {
                    if (item.tag == "Connector")
                    {
                        if (item.GetComponent<WireConnection>().otherConnector != null)
                        {
                            if (item.GetComponent<WireConnection>().otherConnector.transform.parent.GetInstanceID() == transform.parent.GetInstanceID())
                            {
                                same = true;
                            }
                        }
                    }
                }
                if (global.connector.GetComponent<WireConnection>().transform.parent.GetInstanceID() == transform.parent.GetInstanceID() || same)
                {
                    global.connector.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    Debug.Log("Select Same");
                    lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
                    lineRenderer.Simplify(1f);
                    selected = false;
                    resistance = global.wireOhm;
                    //global.wireID = 0; //Unselect
                    global.RedistributePower();

                    length = 0;

                }
                else
                {
                    otherConnector = global.connector; //Other Connector
                    WireConnection otherWire = otherConnector.GetComponent<WireConnection>();
                    otherWire.otherConnector = this.gameObject; //Other Connector's Connector is this
                    resistance = global.wireOhm;
                    otherWire.resistance = global.wireOhm;
                    //Debug.Log("Connection Made");
                    global.RedistributePower();
                    selected = false;

                    //CAUSES: NullReferenceException
                    //length = (int)(global.connector.transform.position - transform.position).magnitude;

                }
            }
            else //First Connector
            {
                global.connector = this.gameObject;
                global.connector.GetComponent<Renderer>().material.SetColor("_Color", lineRenderer.startColor);
                lineRenderer.SetPosition(0, transform.position);
                //Debug.Log("Set Connector!");
                selected = true;

                length = (int)(global.connector.transform.position - transform.position).magnitude;

            }
        }

    }

    public int getResistance() {return resistance;}
    public int getWireType() {return wireType;}

    public GameObject getParent() { return parent; }
}
