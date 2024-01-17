using UnityEngine;
using UnityEngine.UI;

public class BeatPulseRect : MonoBehaviour
{
    public float pulseAmount;
    public float shrinkAmount;
    Vector2 originalSize;
    RectTransform pulseTransform;

    private void Start()
    {
        pulseTransform = GetComponent<RectTransform>();
        originalSize = pulseTransform.sizeDelta;
    }
    private void Update()
    {
        if (!gameObject.activeSelf) return;

        if (pulseTransform.sizeDelta.y > originalSize.y)
        {
            
            Shrink();
        }
    }

    public void Pulse()
    {
        pulseTransform.sizeDelta = originalSize * pulseAmount;

        Debug.Log("Pulse");
    }

    void Shrink()
    {
        Debug.Log("Shrinking");
        pulseTransform.sizeDelta = Vector2.Lerp(pulseTransform.sizeDelta, originalSize, Time.deltaTime * shrinkAmount);
    }
}
