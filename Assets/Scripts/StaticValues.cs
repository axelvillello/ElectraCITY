using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValues : MonoBehaviour
{
    private static StaticValues instance;

    public float volume = 50.0f;
    public string seed;
    public int scenario = 0;

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
    }

    public void SetVolume(float vol)
    {
        volume = vol;
        AudioSource[] sources = this.GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            source.volume = volume/100;
        }
    }
}
