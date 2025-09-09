using UnityEngine;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private Transform FinalScoreBoard;
    private TextMeshProUGUI BaseScore, WireScore, BuildingCost, FinalScore; 
    private Global global;
    private int[] totalScore;
    void Start()
    {
        BaseScore = FinalScoreBoard.Find("BaseScore").GetComponent<TextMeshProUGUI>();
        WireScore = FinalScoreBoard.Find("WireScore").GetComponent<TextMeshProUGUI>();
        BuildingCost = FinalScoreBoard.Find("BuildingCost").GetComponent<TextMeshProUGUI>();
        FinalScore = FinalScoreBoard.Find("FinalScore").GetComponent<TextMeshProUGUI>();

        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>();
    }

    void Update()
    {
        ResultScreenScoring();
    }

    public void ResultScreenScoring()
    {
        totalScore = global.getTotalScore();
        var wireTotals = global.getWireScores();

        BaseScore.text = "Income: " + totalScore[0];
        WireScore.text = "Black Wires: " + wireTotals.black + " x -1 = -" + wireTotals.black + "\n"
        + "Red Wires: " + wireTotals.red + " x -3 = -" + wireTotals.red*3 + "\n"
        + "Yellow Wires: " + wireTotals.yellow + " x -10 = -" + wireTotals.yellow*10;
        //BuildingCost.text =
        FinalScore.text = "Final Score = " + (totalScore[0] - totalScore[1]);   //Placeholder until building cost is implemented
    }
}
