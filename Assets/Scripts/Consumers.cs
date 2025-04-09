using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Consumers : MonoBehaviour
{
    private int pointValue;
    private bool PowerOn;
    private int PowerPercent;
    private bool inGen;
    private Global global;
    private System.Random random;

    private List<GameObject> consumers;
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] GameObject source;
    [SerializeField] Sprite[] sprites;

    private void Awake()
    {
        //Instatiate Variables
        PowerOn = false; //Default Draw
        
        inGen = false;
    }

    private void Start()
    {
        random = new System.Random();
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>();
        tmp.text = pointValue.ToString();
        transform.GetComponent<ConnectorGen>().GenerateConnectors();
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[random.Next(sprites.Length-1)];
    }

    public void reDraw()
    {
        if (isPowerOn())
        {
            source.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            source.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    public void setPoint(int pointValue) 
    { 
        this.pointValue = pointValue;
        tmp.text = pointValue.ToString();
    }
    public int getPoint() { return pointValue; }

    public bool getInGen()
    {
        return inGen;
    }

    public void setInGen(bool i)
    {
        inGen = i;
    }

    public List<GameObject> getConsumers()
    {
        return consumers;
    }

    public bool isPowerOn() { return PowerOn; }
    public void setPowerOn(bool power)
    {
        if (PowerOn != power)
        {
            PowerOn = power;
            reDraw();
        }
    }

    public int getPowerPercent() {  return PowerPercent; }
    public void addPowerPercent(int percent)
    {
        PowerPercent += percent;
        if(PowerPercent >= 99)
        {
            setPowerOn(true);
        }
        else
        {
            setPowerOn(false);
        }
        Debug.Log(getPowerPercent());
    }

    public void PowerReset()
    { 
        PowerPercent = 0;
        setPowerOn(false);
    }

    public int getScore()
    {
        return pointValue;
    }
}
