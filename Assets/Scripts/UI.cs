using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//A system to add a score tally and display a value accodring to a global score variable
public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI progress;
    private Global global;
    private int[] totalScore;

    private void Start()
    {
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>();
    }

    void Update()
    {
        totalScore = global.getTotalScore();
        score.text = "Score: " + (totalScore[0] - totalScore[1]) + "\n" + "Income: " + totalScore[0] + " | Cost: " + totalScore[1];
        progress.text = global.getTotalPoweredCons() + "/" + global.getTotalConsumers() + "\n" + " Powered";
    }
}
