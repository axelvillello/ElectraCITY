using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorGen : MonoBehaviour
{
    public int ConnectorNumber;
    public GameObject[] Connectors;
    public GameObject ConnectorPrefab;

    public void setConNumber(int conNumber)
    {
        ConnectorNumber = conNumber;
    }
    public int getConNumber() { return ConnectorNumber; }

    //Generate the Connectors on consumer objects according to a variable on the consumer
    public void GenerateConnectors()
    {
        Connectors = new GameObject[ConnectorNumber];
        float currentPosx = transform.position.x; //Centered with consumer
        float currentPosy = transform.position.y - 75f; //Below consumer
        float currentPosz = transform.position.z;
        GameObject tempConnector;

        //Populate Dependents
        int flip = 1;
        int distApart = 50;
        for (int i = 0; i < ConnectorNumber; i++)
        {
            if (ConnectorNumber % 2 == 1) //Odd
            {
                tempConnector = Instantiate(ConnectorPrefab, new Vector3(currentPosx + (((i + 1) / 2) * distApart * flip), currentPosy, currentPosz), Quaternion.identity, this.transform);            
            }
            else //Even
            {
                tempConnector = Instantiate(ConnectorPrefab, new Vector3(currentPosx + (((i + 1) / 2) * distApart * flip) + distApart / 2, currentPosy, currentPosz), Quaternion.identity, this.transform);
            }
            flip = flip * -1;
            tempConnector.GetComponent<LineRenderer>().widthMultiplier = 50.0f;
            Connectors[i] = tempConnector;
        }
    }
}
