//Name: Colorblind Manager
//Description: Attached to main camera to render filters. Defines handling of color filter inputs

using UnityEngine;
using TMPro;
using Wilberforce;

public class ColorblindManager : MonoBehaviour
{
    private StaticValues staticValues;
    public TMP_Dropdown inputColorFilter;
    public Camera mainCamera;
    private Colorblind colorFilter;

    void Awake()
    {
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        colorFilter = mainCamera.GetComponent<Colorblind>();

        inputColorFilter.onValueChanged.AddListener(UpdateColorFilter);
    }

    void Update()
    {
        //Check for color filter value each frame
        switch (staticValues.colorFilter)
        {
            case 0: //Default (off)
                colorFilter.Type = 0;
                break;

            case 1: //Red filter
                colorFilter.Type = 1;
                break;

            case 2: //Green filter
                colorFilter.Type = 2;
                break;

            case 3: //Blue filter
                colorFilter.Type = 3;
                break;
        }

        inputColorFilter.RefreshShownValue();
    }

    //Call for changing the color filter value
    private void UpdateColorFilter(int input)
    {

        switch (input)
        {
            case 0:
                staticValues.colorFilter = 0;
                break;

            case 1:
                staticValues.colorFilter = 1;
                break;

            case 2:
                staticValues.colorFilter = 2;
                break;

            case 3:
                staticValues.colorFilter = 3;
                break;
        }
    }

}