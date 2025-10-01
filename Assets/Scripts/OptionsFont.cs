using UnityEngine;
using TMPro;

public class OptionsFont : MonoBehaviour
{
    public TMP_Dropdown inputFont;
    public FontManager fontManager;
    private void Awake()
    {
        fontManager = GameObject.FindFirstObjectByType<FontManager>();
        inputFont.onValueChanged.AddListener(fontManager.FontSelection);
    }
}