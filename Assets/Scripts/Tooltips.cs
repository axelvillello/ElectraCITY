using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class Tooltips: MonoBehaviour
{
    [Header("Tooltips Settings")]
    [SerializeField] private string tooltipContent;
    [SerializeField] private Transform tooltipTransform;
    private TextMeshProUGUI tooltipValue;
    private bool tooltipActive = false;
    private Canvas canvas;
    void Awake()
    {
        tooltipValue = tooltipTransform.GetComponentInChildren<TextMeshProUGUI>();
        canvas = FindFirstObjectByType<Canvas>();
    }
    void Update()
    {
        if (tooltipActive)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            tooltipTransform.localPosition = localPoint + new Vector2(150f, 20f);
            tooltipTransform.gameObject.SetActive(true);
        }
    }
    public void ActivateTooltip()
    {
        StartCoroutine(ShowTooltipAfterDelay());
        
    }

    private IEnumerator ShowTooltipAfterDelay()
    {
        tooltipValue.text = tooltipContent;
        yield return new WaitForSeconds(0.5f);
        tooltipActive = true;
    }

    public void DeactivateTooltip()
    {
        StopAllCoroutines();
        tooltipTransform.gameObject.SetActive(false);
        tooltipActive = false;
    }
    
}