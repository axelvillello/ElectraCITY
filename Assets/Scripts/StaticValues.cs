using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticValues : MonoBehaviour
{
    private static StaticValues instance;

    public float volume;
    public string seed;
    public int scenario = 0;
    public float textSpd = 5;
    public int colorFilter = 0;
    public string textFont;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        volume = 50.0f;
        textFont = "ThaleahFat_TTF";
    }

    public void SetVolume(float vol)
    {
        volume = vol;
        AudioSource[] sources = this.GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            source.volume = volume / 100;
        }
    }

    public void SetTxtSpd(float spd)
    {
        textSpd = spd;
    }

    public void SetFont(string fnt)
    {
        textFont = fnt;
    }
}
