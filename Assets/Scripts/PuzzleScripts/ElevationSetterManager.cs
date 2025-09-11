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

    private void InvertETriggers(ElevationSetter justTriggered)
    {
        if(justTriggered.gameObject == top)
        {
            top.GetComponent<ElevationSetter>().DisableETrigger();
            bottom.GetComponent<ElevationSetter>().EnableETrigger();
        }
        else
        {
            top.GetComponent<ElevationSetter>().EnableETrigger();
            bottom.GetComponent<ElevationSetter>().DisableETrigger();
        }
    }
}
