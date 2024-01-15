using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatPulse : MonoBehaviour
{
    public float pulseAmount;
    public float shrinkAmount;
    Vector3 originalSize;

    private void Start()
    {
        originalSize = transform.localScale;
    }

    private void Update()
    {
        if (transform.localScale.magnitude > originalSize.magnitude)
        {
            Shrink();
        }
    }

    public void Pulse()
    {
        transform.localScale = originalSize * pulseAmount;
        Debug.Log("pulsed");
    }

    void Shrink()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalSize, Time.deltaTime * shrinkAmount);
    }
}
