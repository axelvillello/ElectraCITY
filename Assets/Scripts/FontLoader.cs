//Name: Font Loader
//Description: Handles the loading of fonts on objects inactive in a scene

using UnityEngine;

public class FontLoader : MonoBehaviour
{
    private FontManager fontManager;

    private void OnEnable()
    {
        fontManager = FindFirstObjectByType<Camera>().GetComponent<FontManager>();
        fontManager.UpdateFont();
    }
}