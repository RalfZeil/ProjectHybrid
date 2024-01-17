
using UnityEngine;
using System.Collections;

public class FlickeringLights : MonoBehaviour
{
    public MonoBehaviour targetScript;
    public float minWaitTime;
    public float maxWaitTime;

    void Start()
    {
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            ToggleScript();
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            ToggleScript();
        }
    }

    void ToggleScript()
    {
        if (targetScript != null)
        {
            targetScript.enabled = !targetScript.enabled;
        }
    }
}