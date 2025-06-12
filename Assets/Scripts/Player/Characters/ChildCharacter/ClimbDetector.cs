using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbDetector : CheckCollision
{
    public bool canClimb = false;
    private Collider2D _climbableCollider;

    public Collider2D Climbable => _climbableCollider; //Prob Publica

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable") || collision.CompareTag("Pushable"))
        {
            canClimb = true;
            _climbableCollider = collision;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable") || collision.CompareTag("Pushable"))
        {
            canClimb = false;
            _climbableCollider = null;
        }
    }
}
