//Name: Text Speed Manager
//Description: Handles the changing of text speed

using UnityEngine;
using UnityEngine.UI;

public class TxtSpeedManager : MonoBehaviour
{
    private StaticValues staticValues;
    public Slider inputTextSpd;
    void Awake()
    {
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        inputTextSpd.value = staticValues.textSpd;
        inputTextSpd.onValueChanged.AddListener(UpdateTextSpeed);
    }

    public void UpdateTextSpeed(float speed)
    {
        staticValues.textSpd = speed;
    }

}