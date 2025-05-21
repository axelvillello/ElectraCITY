using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LiveVolume : MonoBehaviour
{
    [SerializeField] GameObject textObj;
    private TMP_Text textField;
    public AudioSource click;

    public Slider volumeSlider;
    // Start is called before the first frame update
    private void Start()
    {
        textField = textObj.GetComponent<TMP_Text>();
        textField.text = (this.GetComponent<Slider>().value).ToString();
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        click.volume = this.GetComponent<Slider>().value;
    }

    void ValueChangeCheck()
    {
        textField.text = (this.GetComponent<Slider>().value).ToString();
        click.volume = this.GetComponent<Slider>().value/100;
        click.Play();
    }

}
