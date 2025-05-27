using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidableSurfaceDetector : MonoBehaviour
{
  //Chequeo si el objeto asignado esta sobre una superficie deslizable.
    public bool IsOnSlidableSurface { get; private set; }

    [SerializeField] private LayerMask slidableLayer;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & slidableLayer) != 0)
        {
            IsOnSlidableSurface = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & slidableLayer) != 0)
        {
            IsOnSlidableSurface = false;
        }
    }
}
