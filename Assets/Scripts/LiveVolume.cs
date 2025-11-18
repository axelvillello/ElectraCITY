//Name: Live Volume
//Description: Handles the updating of volume for audio in the UI

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiveVolume : MonoBehaviour
{
    [SerializeField] GameObject textObj;
    private TMP_Text textField;
    public AudioSource click;
    private StaticValues staticValues;
    public Slider volumeSlider;
    
    private void Start()
    {
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        textField = textObj.GetComponent<TMP_Text>();
        volumeSlider.value = staticValues.volume;
        textField.text = this.GetComponent<Slider>().value.ToString();
        click.volume = this.GetComponent<Slider>().value;
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    //Updates the volume level 
    void ValueChangeCheck()
    {
        float newVolume = volumeSlider.value;
        textField.text = newVolume.ToString();
        staticValues.SetVolume(newVolume);
        click.volume = newVolume/100;
        click.Play();
    }

}
