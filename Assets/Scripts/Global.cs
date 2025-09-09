using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Global : MonoBehaviour
{
    private static Global instance;
    private GameObject[] Consumers;
    private GameObject[] Generators;
    private GameObject[] Connectors;
    private Camera camera;
    private GameObject[] wires;
    private int[] totalScore;
    private int consumersOn = 0;

    private StaticValues staticValues;
    private int seed;
    public Boolean wireSelected;

    public GameObject connector;
    public GameObject finishBtn;
    public Image uiImage;
    private Color originalBGColor;
    public int wireOhm;
    public int wireID = 0; 
    //0: None Selected
    //1: Black Wire
    //2: Red Wire
    //3: Yellow Wire

    [SerializeField] GameObject ConsumerPrefab;
    [SerializeField] GameObject GeneratorPrefab;
    [SerializeField] GameObject BGObject;
    [SerializeField] GameObject Tutorial;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            uiImage = GameObject.FindGameObjectWithTag("Background").GetComponent<Image>();
            originalBGColor = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b);
            //DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        if(staticValues.seed == null)
        {
            staticValues.seed = Random.Range(0, 1000000).ToString();
        }
        seed = Math.Abs(staticValues.seed.GetHashCode());
        Debug.Log(seed);
        
        Random.InitState(seed); //Seed
        Scenario ChosenScenario = ScenarioList(staticValues.scenario);
        camera = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>().worldCamera;
        wires = GameObject.FindGameObjectsWithTag("Wire");
        
        totalScore = new int[2];

        Generate(ChosenScenario);

        Consumers = GameObject.FindGameObjectsWithTag("Consumer");
        Generators = GameObject.FindGameObjectsWithTag("Generator");
        Connectors = GameObject.FindGameObjectsWithTag("Connector");

    }

    private Scenario ScenarioList(int pickedScenario)
    {
        Scenario scenario = new Scenario();
        switch (pickedScenario)
        {
            case 0:
                //Scenario 0 (Base Scenario)
                scenario.setGen(4);
                scenario.setCon(30);
                break;
            case 1:
                //Scenario 1 (No Super, Only 2 Gens)
                scenario.setGen(2);
                scenario.setCon(20);
                scenario.setYellow(false);
                break;
            case 2:
                //Scenario 2
                scenario.setGen(4);
                scenario.setCon(20);
                scenario.setYellow(false);
                scenario.setRed(false);
                break;
            case 100:
                //Tutorial
                scenario.setGen(4);
                scenario.setCon(30);
                Tutorial.SetActive(true);   //May update to a tutorial class object insteads
                break;
            default:
                //Fallen Through
                Debug.Log("SCENARIO NOT RECOGNISED!");
                break;
        }
        return scenario;
    }

    private void Update()
    {

        if (connector)
        {
            uiImage.color = new Color(originalBGColor.r, originalBGColor.g, originalBGColor.b, 0.4f);
            Debug.Log("BG color dimmed!");
        }
        else
        {
            uiImage.color = new Color(originalBGColor.r, originalBGColor.g, originalBGColor.b);
            Debug.Log("BG color reverted!");
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1)) //Right Click
        {
            if (connector)
            {
                connector.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                Debug.Log("Connector changed to white!");
            }
            RedistributePower();
        }

        
    }

    public void RedistributePower()
    {
            connector = null;
            PowerReset();
            for (int i = 0; i < GetGeneratorsLength(); i++)
            {
                GetGeneratorsIndex(i).GetComponent<Generators>().Plant();
                ConsumerClear();
            }
            ConsumerChange();
    }

    public void ConsumerClear()
    {
        for(int i = 0; i < Consumers.Length; i++)
        {
            Consumers[i].GetComponent<Consumers>().setInGen(false);
        }
    }

    public void PowerReset()
    {
        for (int i = 0; i < Consumers.Length; i++)
        {
            Consumers[i].GetComponent<Consumers>().PowerReset();
        }
    }

    public void Generate(Scenario scenario)
    {

        GridObject[,] grid = new GridObject[36,20];
        grid = GridManager(grid);

        foreach (GridObject gridSpace in grid)
        {

            if (Random.Range(0,100) < 15 && gridSpace.getY() > 100)
            {
                GameObject obj = Instantiate(BGObject, camera.ScreenToWorldPoint(new Vector3(gridSpace.getX(), gridSpace.getY(), 120)), Quaternion.identity, this.transform);
                obj.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteLibrary>().GetSprite("BGObjects", Random.Range(1,4).ToString());
            }
        }

        for (int i = 0; i < wires.Length; i++)
        {
            if (wires[i].name == "S-Wire" && !scenario.getYellow()) { wires[i].SetActive(false); }
            if (wires[i].name == "R-Wire" && !scenario.getRed()) { wires[i].SetActive(false); }
            if (wires[i].name == "B-Wire" && !scenario.getBlack()) { wires[i].SetActive(false); }
        }

        for(int gens = 0; gens < scenario.getGen(); gens++)
        {
            bool notValid = true;
            int x = 0;
            int y = 0;
            while (notValid)
            {
                x = Random.Range(1, 35);
                y = Random.Range(3, 19);
                if(CheckSpacing(grid, x, y))
                {
                    notValid = false;
                }
            }
            Debug.Log("x: " + x + " Y: " + y);
            grid[x,y].setResource(Instantiate(GeneratorPrefab, camera.ScreenToWorldPoint(new Vector3(grid[x,y].getX(), grid[x,y].getY(), 110)), Quaternion.identity, this.transform));
            NearbyOccupied(grid, x, y);
            //Connector Number
            int rand = Random.Range(0, 100);
            if (rand <= 15)
            {
                grid[x, y].getResource().GetComponent<ConnectorGen>().setConNumber(1);
            }
            else if(rand <= 55)
            {
                grid[x, y].getResource().GetComponent<ConnectorGen>().setConNumber(2);
            }
            else if(rand <= 95)
            {
                grid[x, y].getResource().GetComponent<ConnectorGen>().setConNumber(3);
            }
            else if (rand <= 100)
            {
                grid[x, y].getResource().GetComponent<ConnectorGen>().setConNumber(4);
            }

        }

        for (int cons = 0; cons < scenario.getCon(); cons++)
        {
            bool notValid = true;
            int x = 0;
            int y = 0;
            while (notValid)
            {
                x = Random.Range(2, 35);
                y = Random.Range(3, 19);
                if (CheckSpacing(grid, x, y))
                {
                    notValid = false;
                }
            }
            Debug.Log("x: " + x + " Y: " + y);
            grid[x, y].setResource(Instantiate(ConsumerPrefab, camera.ScreenToWorldPoint(new Vector3(grid[x, y].getX(), grid[x, y].getY(), 110)), Quaternion.identity, this.transform));
            NearbyOccupied(grid, x, y);
            //Connector Number
            int rand = Random.Range(0, 100);
            ConnectorGen conObj = grid[x, y].getResource().GetComponent<ConnectorGen>();
            if (rand <= 15)
            {
                conObj.setConNumber(1);
            }
            else if (rand <= 55)
            {
                conObj.setConNumber(2);
            }
            else if (rand <= 95)
            {
                conObj.setConNumber(3);
            }
            else if (rand <= 100)
            {
                conObj.setConNumber(4);
            }
            //Scoring
            double score = 5.0;
            if (x <= 5 || x >= 29) //Edge Bonus
            {
                score *= 1.25;
            }
            if (y <= 5 || y >= 17) //Edge Bonus
            {
                score *= 1.25;
            }
            if (20 < x && x < 30) //Center Demerit
            {
                score *= 0.8; 
            }
            if(10 <y && y < 14)
            {
                score *= 0.8;
            }
            //Note if both true corner bonus of 1.5625
            if (conObj.getConNumber() == 1)
            {
                score *= 1.5;
            }
            if (conObj.getConNumber() == 2)
            {
                score *= 1.25;
            }
            if (conObj.getConNumber() == 3)
            {
                score *= 0.95;
            }
            if (conObj.getConNumber() == 4)
            {
                score *= 0.8;
            }
            grid[x, y].getResource().GetComponent<Consumers>().setPoint((int)score);
        }
    }

    public bool CheckSpacing(GridObject[,] grid, int x, int y)
    {
        if (grid[x, y].isOccupied() || grid[x+1, y].isOccupied() || grid[x-1, y].isOccupied() || grid[x, y-1].isOccupied() || grid[x+1, y-1].isOccupied() || grid[x-1, y-1].isOccupied())
        {
            Debug.Log("Occupied at x:" + x + " y:" + y);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void NearbyOccupied(GridObject[,] grid, int x, int y)
    {
        grid[x, y].flipOccupied();
        grid[x + 1, y].flipOccupied();
        grid[x - 1, y].flipOccupied();
        grid[x, y - 1].flipOccupied();
        grid[x + 1, y - 1].flipOccupied();
        grid[x - 1, y - 1].flipOccupied();
    }

    public GridObject[,] GridManager(GridObject[,] grid)
    {
        float width = camera.pixelWidth/grid.GetLength(0);
        float height = camera.pixelHeight/grid.GetLength(1);
        int[] coords = new int[2];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                coords[0]=x;
                coords[1]=y;
                grid[x, y] = new GridObject(x*width+width/2, y*height+height/2, width, height, coords);
            }
        }
        return grid;
    }

    public void ConsumerChange()
    {
        int on = 0;
        for(int i = 0; i<Consumers.Length-1; i++)
        {
            if (Consumers[i].GetComponent<Consumers>().isPowerOn())
            {
                on += 1;
            }
        }
        if (consumersOn < on)
        {
            staticValues.GetComponent<AudioManager>().Play("PowerOn");
        }
        else if (consumersOn > on)
        {
            staticValues.GetComponent<AudioManager>().Play("PowerOff"); 
        }
        consumersOn = on;
        //GameEndCheck(on);
    }

    //Finish button appearance, may not be needed 
    /*
    public void GameEndCheck(int PoweredCons)
    {
        if((PoweredCons >= 1 && finishBtn.activeSelf == false) || (PoweredCons < 1 && finishBtn.activeSelf == true)) //Variable not set to Consumers.Length for testing purposes
        {
            finishBtn.SetActive(!finishBtn.activeSelf);
        }
    }
    */

    public void Play(String sound)
    {
        staticValues.GetComponent<AudioManager>().Play(sound);
    }
    public GameObject[] GetConsumers() {return Consumers;}
    public GameObject[] GetGenerators() {return Generators;}
    public int GetConsumersLength() { return Consumers.Length;}
    public int GetGeneratorsLength() { return Generators.Length; }
    public GameObject GetConsumersIndex(int i) { return Consumers[i]; }
    public GameObject GetGeneratorsIndex(int i) { return Generators[i]; }

    public int[] getTotalScore() 
    {
        totalScore[0] = 0;
        totalScore[1] = 0;
        foreach (GameObject con in Consumers)
        {
            Consumers conSc = con.GetComponent<Consumers>();
            if (conSc.isPowerOn())
            {
                GameObject[] scConnectors = conSc.transform.GetComponent<ConnectorGen>().Connectors;
                foreach (GameObject scCon in scConnectors)
                {
                    int scConLen = scCon.GetComponent<WireConnection>().length;
                    if (scCon.GetComponent<WireConnection>() != null || scConLen != 0)
                    {
                        totalScore[0] += conSc.getScore() * scConLen;
                    }
                }         
            }
        }
        foreach (GameObject gen in Generators)
        {
            Generators genSc = gen.GetComponent<Generators>();
            totalScore[1] += genSc.getTotalConnectionsCost();
        }
        return totalScore;
    }

    public (int black, int red, int yellow) getWireScores()     
    {
        int black = 0 , red = 0, yellow = 0;
        foreach (GameObject gen in Generators)
        {
            Generators genSc = gen.GetComponent<Generators>();
            var wireTotals = genSc.getTotalWires();
            black += wireTotals.black;
            red += wireTotals.red;
            yellow += wireTotals.yellow;
        }
        return (black, red, yellow);
    }
    public int getTotalPoweredCons(){return consumersOn;}
    public int getTotalConsumers(){return Consumers.Length;}
}   