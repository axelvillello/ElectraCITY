using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject ScreenToOpen;
    private StaticValues staticValues;
    private AudioManager audio;
    private ResultScreen ScoreDisplay;
    private RectTransform btn;
    private Vector3 originalSize, hoveredSize;
    private GameObject self;


    private void Start()
    {
        self = this.gameObject;
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        audio = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<AudioManager>();

        //Values required for button shrinking effect
        btn = GetComponent<RectTransform>();
        originalSize = btn.localScale;
        hoveredSize = new Vector3(originalSize.x*0.9f, originalSize.y*0.9f, originalSize.z*0.9f);
    }


    public void OnStartButton()
    {
        ScreenToOpen.SetActive(!ScreenToOpen.activeSelf);
        audio.Play("MenuClick");
    }

    public void OnTutorialButton()
    {
        staticValues.seed = "A_Nice_Tutorial";
        staticValues.scenario = 100;
        audio.Play("MenuClick");
        SceneManager.LoadScene(1);
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
        //staticValues.SetVolume(ScreenToOpen.GetComponentInChildren<Slider>().value);
        ScreenToOpen.SetActive(!ScreenToOpen.activeSelf);
        audio.Play("MenuClick");
    }

    public void OnFinishButton()
    {
        ScreenToOpen.SetActive(!ScreenToOpen.activeSelf);
        audio.Play("MenuClick");
    }

    public void OnFinishConfirmButton()
    {
        ScreenToOpen.SetActive(!ScreenToOpen.activeSelf);
        GameObject finishBtn = GameObject.FindGameObjectWithTag("Finish");
        finishBtn.SetActive(false);
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

    public void OnMouseOver()
    {
        self.transform.GetComponent<SquashAndStretch>().PlaySquashAndStretchEffect();
        
    }

    public void OnMouseExit()
    {
        self.transform.GetComponent<SquashAndStretch>().PlaySquashAndStretchEffect();
    }
}
