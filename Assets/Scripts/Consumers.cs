using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Consumers : MonoBehaviour
{
    private int pointValue;
    private bool PowerOn;
    private int PowerPercent;
    private bool inGen;
    private int genSpriteID;
    private Global global;
    private System.Random random;

    private List<GameObject> consumers;
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] GameObject source;
    [SerializeField] Sprite[] lightbulbSprites;
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
        genSpriteID = checkSprite(pointValue);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[genSpriteID];
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = randomColor(this.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
    }

    public void reDraw()
    {
        if (isPowerOn())
        {
            source.GetComponent<SpriteRenderer>().sprite = lightbulbSprites[1];
        }
        else
        {
            source.GetComponent<SpriteRenderer>().sprite = lightbulbSprites[0];
        }
    }

    public void setPoint(int pointValue)
    {
        this.pointValue = pointValue;
        tmp.text = pointValue.ToString();
    }
    private int checkSprite(int comparedPoints)
    {
        int spriteID = 0;
        List<int> numbers = null;  //Holds ID of sprites that represent the same point value
        switch (comparedPoints)
        {
            case 3:
                spriteID = 11;
                break;
            case 4:
                numbers = new List<int> { 4, 6, 7 };
                spriteID = random.Next(numbers.Count);
                break;
            case 5:
                numbers = new List<int> { 1, 2, 12 };
                spriteID = random.Next(numbers.Count);
                break;
            case 6:
                spriteID = 10;
                break;
            case 7:
                numbers = new List<int> { 0, 3 };
                spriteID = random.Next(numbers.Count);;
                break;
            case 8:
                numbers = new List<int> { 5, 8 };
                spriteID = random.Next(numbers.Count);
                break;
            case 9:
                spriteID = 9;
                break;
            default:
                Debug.Log("Invalid point value for consumer object!");
                break;
        }
        if (numbers != null)
        { 
            numbers.Clear();
        }
        return spriteID;
    }

    //Applies a random color tint to the sprite
    public Color randomColor(Color originalColor)
    {
        Color randomColor = new Color(Random.Range(0.7f, 1f), Random.Range(0.7f, 1f), Random.Range(0.7f, 1f), 1f);
        randomColor = originalColor * randomColor;

        return randomColor;
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
