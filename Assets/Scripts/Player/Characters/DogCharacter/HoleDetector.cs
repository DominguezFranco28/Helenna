using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDetector : MonoBehaviour
{
    private bool _canDig;
    public bool CanDig => _canDig;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //I turn off the fence collider when the dog detects the hole
        if (collision.CompareTag("Hole"))
        {
            Transform parent = collision.transform.parent;
            if (parent != null)
            {
                Collider2D parentCollider = parent.GetComponent<Collider2D>();
                if (parentCollider != null)
                {
                    parentCollider.enabled = false;
                }
            }
            _canDig = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hole"))
        {
            Transform parent = collision.transform.parent;
            if (parent != null)
            {
                Collider2D parentCollider = parent.GetComponent<Collider2D>();
                if (parentCollider != null)
                {
                    parentCollider.enabled = true;
                }
            }
            _canDig = false;
        }
    }
}

