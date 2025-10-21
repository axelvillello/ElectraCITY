using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;


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
        //A system to add a score tally and display a value according to a global score variable
        totalScore = global.getTotalScore();
        score.text = "SCORE: " + (totalScore[0] - totalScore[1]) + "\n" + "Cost: " + totalScore[1];
        progress.text = global.getTotalPoweredCons() + "/" + global.getTotalConsumers() + "\n" + " Powered";
    }

}
