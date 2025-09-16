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
        switch (staticValues.colorFilter)
        {
            case 0:
                colorFilter.Type = 0;
                break;

            case 1:
                colorFilter.Type = 1;
                break;

            case 2:
                colorFilter.Type = 2;
                break;

            case 3:
                colorFilter.Type = 3;
                break;
        }

        inputColorFilter.RefreshShownValue();
    }

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