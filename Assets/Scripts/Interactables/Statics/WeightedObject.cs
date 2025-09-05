using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{
    [SerializeField] private float baseWeight = 5f;
    public bool autoCalculateWeight = false;

    public float GetWeight()
    {
        if (autoCalculateWeight)
            return baseWeight * transform.localScale.x * transform.localScale.y * transform.localScale.z;
        else
            return baseWeight;
    }
}
