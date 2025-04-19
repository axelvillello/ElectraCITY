using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject ScreenToOpen;
    private StaticValues staticValues;
    private AudioManager audio;

    private void Start()
    {
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        audio = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<AudioManager>();
    }


    public void OnStartButton()
    {
        ScreenToOpen.SetActive(!ScreenToOpen.activeSelf);
        audio.Play("MenuClick");
    }

    public void OnOptionsButton()
    {
        ScreenToOpen.GetComponentInChildren<Slider>().value = staticValues.volume;
        ScreenToOpen.SetActive(!ScreenToOpen.activeSelf);
        audio.Play("MenuClick");
    }

    public void OnExitButton()
    {
        Application.Quit();
        audio.Play("MenuClick");
    }

    public void BackButton()
    {
        ScreenToOpen.SetActive(!ScreenToOpen.activeSelf);
        audio.Play("MenuClick");
    }

    public void OnStartStartButton()
    {
        staticValues.seed = ScreenToOpen.GetComponentInChildren<TMP_InputField>().text;
        staticValues.scenario = ScreenToOpen.GetComponentInChildren<TMP_Dropdown>().value;
        audio.Play("MenuClick");
        SceneManager.LoadScene(1);
    }
    public void OnOptionsApplyButton()
    {
        staticValues.SetVolume(ScreenToOpen.GetComponentInChildren<Slider>().value);
        audio.Play("MenuClick");
    }

    public void OnFinishButton()
    {
        ScreenToOpen.SetActive(!ScreenToOpen.activeSelf);
        audio.Play("MenuClick");
    }

    public void OnReturnToMenuButton()
    {
        audio.Play("MenuClick");
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        //SceneManager.UnloadSceneAsync(1);
    }

    public void OnRetryButton()
    {
        audio.Play("MenuClick");
        SceneManager.LoadScene(1);
    }

}
