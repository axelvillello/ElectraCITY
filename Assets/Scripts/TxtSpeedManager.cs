using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TxtSpeedManager : MonoBehaviour
{
    private StaticValues staticValues;
    public TMP_InputField inputTextSpd;
    void Awake()
    {
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        inputTextSpd.text = staticValues.textSpd.ToString();

        inputTextSpd.onEndEdit.AddListener(UpdateTextSpeed);
    }

    void Update()
    {
        if (staticValues.textSpd > 10)
        {
            staticValues.textSpd = 10;
            inputTextSpd.text = staticValues.textSpd.ToString();
        }
        else if (staticValues.textSpd == 0 || staticValues.textSpd < 0)
        { 
            staticValues.textSpd = 1;
            inputTextSpd.text = staticValues.textSpd.ToString();
        }
    }

    private void UpdateTextSpeed(string input)
    {
        int speed;

        if (int.TryParse(input, out speed) && speed > 0)
        {
            staticValues.textSpd = speed;
        }
        else
        {
            staticValues.textSpd = 5;
        }
    }

    public void OnPlusButton()
    {
        staticValues.textSpd++;
        inputTextSpd.text = staticValues.textSpd.ToString();
    }

    public void OnMinusButton()
    {
        staticValues.textSpd--;
        inputTextSpd.text = staticValues.textSpd.ToString();
    }

}