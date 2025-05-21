using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireAttach : MonoBehaviour
{
    //UI Wire Element control
    public enum WireType
    {
        Black,
        Red,
        Super
    }
    [SerializeField] WireType wireType;

    private Global global;
    private int wireOhm;
    private int wireId;
    private Boolean selected;

    public Sprite unpressed;
    public Sprite pressed;
    private Image current;

    private void Awake()
    {
        //Setting Wire type
        selected = false;
        current = GetComponent<Image>();
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>();
        switch (wireType)
        {
            case WireType.Black:
                wireId = 1;
                wireOhm = 3;
                break;
            case WireType.Red:
                wireId = 2;
                wireOhm = 1;
                break;
            case WireType.Super:
                wireId = 3;
                wireOhm = 0;
                break;
        }
    }

    private void Update()
    {
        //Change Global activated type
        if (global.wireID != wireId)
        {
            current.sprite = unpressed;
            selected = false;
        }
        else
        {
            current.sprite = pressed;
            selected = true;
        }
    }

    public void Attach()
    {   
        if (!global.wireSelected)
        {
            global.Play("Click1");
            //Global set of which wire selected
            if (!selected)
            {
                global.wireID = wireId;
                global.wireOhm = wireOhm;
                current.sprite = pressed;
                selected = true;
            }
            else if (global.connector != null)
            {
                global.wireID = 0;
                selected = false;
            }
            else
            {
                global.wireID = 0;
                global.wireSelected = false;
            }
        }
    }
}
