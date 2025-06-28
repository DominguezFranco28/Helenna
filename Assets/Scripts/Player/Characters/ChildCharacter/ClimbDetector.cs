using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbDetector : MonoBehaviour
{
    private bool _canClimb = false;
    private Collider2D _climbableCollider;
    public bool CanClimb => _canClimb;
    public Collider2D Climbable => _climbableCollider;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable") || collision.CompareTag("Pushable"))
        {
            _canClimb = true;
            _climbableCollider = collision;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable") || collision.CompareTag("Pushable"))
        {
            _canClimb = false;
            _climbableCollider = null;
        }
    }
}
