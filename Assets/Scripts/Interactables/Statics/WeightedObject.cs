using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{
    [SerializeField] private float baseWeight = 5f;
    [SerializeField] private float weight = 0f;
    public bool autoCalculateWeight = false;

    private void Start()
    {
        weight = GetWeight();
    }

    public float GetWeight()
    {
        if (autoCalculateWeight)
            return baseWeight * transform.localScale.x * transform.localScale.y * transform.localScale.z;
        else
            return baseWeight;
    }
}
