//Name: Color Loader
//Description: Handles the loading of colorblind filters when reloading the main scene

using UnityEngine;
using Wilberforce;

public class ColorLoader : MonoBehaviour
{
    private FontManager fontManager;
    private StaticValues staticValues;
    public Camera mainCamera;

    private void Start()
    {
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        mainCamera.GetComponent<Colorblind>().Type = staticValues.colorFilter;
    }
}