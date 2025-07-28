using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoleDetector : MonoBehaviour
{
   [SerializeField] private AgilePlayerBehaviour _playerBehaviour;

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
            _playerBehaviour.CanDig = true;
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
            _playerBehaviour.CanDig = false;
        }
    }

}

