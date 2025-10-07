using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class FontManager : MonoBehaviour
{
    private GameObject[] sceneAssets;
    private StaticValues staticValues;
    private Font font;
    private TMP_FontAsset fontAsset;

    private void Awake()
    {
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        sceneAssets = SceneManager.GetActiveScene().GetRootGameObjects();
    }
    void Start()
    {
        ChangeFont();
        UpdateFont();
    }
    void Update()
    {
        
    }

    public void FontSelection(int input)
    {
        switch (input)
        {
            case 0:
                staticValues.textFont = "ThaleahFat_TTF";
                break;

            case 1:
                staticValues.textFont = "LiberationSans";
                break;

        }
        ChangeFont();
        UpdateFont();
    }
    private void ChangeFont()
    {
        font = Resources.Load<Font>(staticValues.textFont);
        fontAsset = TMP_FontAsset.CreateFontAsset(font);
    }

    public void UpdateFont()
    {
        foreach (GameObject asset in sceneAssets)
        {
            if (asset.transform.gameObject.CompareTag("Text"))
            {
                asset.transform.gameObject.GetComponent<TMP_Text>().font = fontAsset;
            }

            foreach (Transform child in asset.transform)
            {
                FindAssetWithTag(child);
            }
        }
    }

    public void FindAssetWithTag(Transform parent)  //Recursive function used using Transforms instead of GameObject
    {
        if (parent.gameObject.CompareTag("Text"))
        {
            parent.gameObject.GetComponent<TMP_Text>().font = fontAsset;
        }

        foreach (Transform child in parent)
        {
            FindAssetWithTag(child);
        }
    }

}