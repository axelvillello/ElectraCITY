//Name: Options Font
//Description: Provides method for font options in the UI to communicate with font manager in the scene

using UnityEngine;
using UnityEngine.UI;

public class OptionsFont : MonoBehaviour
{
    [SerializeField] Button Default; // Button for ThaleahFat_TTF
    [SerializeField] Button Dyslexic; // Button for OpenDyslexic-Regular
    private FontManager fontManager;

    private void Awake()
    {
        fontManager = GameObject.FindFirstObjectByType<FontManager>();

        Default.onClick.AddListener(() => fontManager.FontSelection(0));
        Dyslexic.onClick.AddListener(() => fontManager.FontSelection(1));
    }
}