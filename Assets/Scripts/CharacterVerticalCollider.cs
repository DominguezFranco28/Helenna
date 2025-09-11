using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVerticalCollider : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask elevatedLayers;
    [SerializeField] private BoxCollider2D boxCollider;
    
    public bool toggle = false;
    private bool elevated = false;

    private void Start()
    {
        SetToGroundColliders();
    }

    private void Update()
    {
        if (toggle)
        {
            toggle = false;
            if (elevated)
                SetToGroundColliders();
            else
                SetToElevatedColliders();
        }
    }

    public void SetToGroundColliders()
    {
        elevated = false;

        int layerIndex;

        layerIndex = Mathf.RoundToInt(Mathf.Log(elevatedLayers.value, 2));
        IncludeLayer(layerIndex);

        layerIndex = Mathf.RoundToInt(Mathf.Log(groundLayers.value, 2));
        ExcludeLayer(layerIndex);
    }

    public void SetToElevatedColliders()
    {
        elevated = true;

        int layerIndex;

        layerIndex = Mathf.RoundToInt(Mathf.Log(groundLayers.value, 2));
        IncludeLayer(layerIndex);

        layerIndex = Mathf.RoundToInt(Mathf.Log(elevatedLayers.value, 2));
        ExcludeLayer(layerIndex);
    }

    void ExcludeLayer(int layerIndex)
    {
        boxCollider.excludeLayers |= (1 << layerIndex);
    }

    void IncludeLayer(int layerIndex)
    {
        boxCollider.excludeLayers &= ~(1 << layerIndex);
    }
}
