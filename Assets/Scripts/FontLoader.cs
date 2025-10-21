using Unity.VisualScripting;
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