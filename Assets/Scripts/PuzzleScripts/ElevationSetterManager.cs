using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevationSetterManager : MonoBehaviour
{
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;

    private void OnEnable()
    {
        top.GetComponent<ElevationSetter>().OnTriggered += InvertETriggers;
    }

    private void OnDisable()
    {
        top.GetComponent<ElevationSetter>().OnTriggered -= InvertETriggers;
    }

    private void InvertETriggers(ElevationSetter justTriggered)
    {
        StartCoroutine(DelayedInvert(justTriggered));
    }

    private IEnumerator DelayedInvert(ElevationSetter justTriggered)
    {
        if (justTriggered.gameObject == top)
        {
            top.GetComponent<ElevationSetter>().DisableETrigger();
            yield return new WaitForSeconds(0.5f);
            bottom.GetComponent<ElevationSetter>().EnableETrigger();
        }
        else
        {
            top.GetComponent<ElevationSetter>().EnableETrigger();
            yield return new WaitForSeconds(0.5f);
            bottom.GetComponent<ElevationSetter>().DisableETrigger();
        }
    }
}
