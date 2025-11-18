//Name: Font Manager
//Description: Handles font of text of all scene objects

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class FontManager : MonoBehaviour
{
    private GameObject[] sceneAssets;
    private StaticValues staticValues;
    private Font font;
    private TMP_FontAsset fontAsset;
    private readonly System.Collections.Generic.Dictionary<TMP_Text, float> originalSizes = new();

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

    public void FontSelection(int input)
    {
        switch (input)
        {
            case 0:
                staticValues.textFont = "ThaleahFat_TTF";
                break;

            case 1:
                staticValues.textFont = "OpenDyslexic-Regular";
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
        TMP_Text[] allTexts = Object.FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);

        foreach (TMP_Text text in allTexts)
        {
            if (text.CompareTag("Text"))
            {
                //Obtains original (default) fontsize of text object
                if (!originalSizes.ContainsKey(text))
                {
                    originalSizes[text] = text.fontSize;
                }

                text.font = fontAsset;

                //Scales font size if dyslexic friendly font is selected due to font sizing issues
                if (staticValues.textFont == "OpenDyslexic-Regular")
                {
                    text.fontSize = originalSizes[text] * 0.55f;
                }
                else
                {
                    text.fontSize = originalSizes[text];
                }

                //Attempt at adding a white outline to all text 
                var matInstance = fontAsset.material;
                matInstance.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.15f);
                matInstance.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
                text.fontSharedMaterial = matInstance;
            }
        }
    }
}