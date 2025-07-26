using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerDetector : MonoBehaviour
{
    private bool _canClimb = false;
    private Collider2D _climbableCollider;
    private bool _isCooldownActive = false;

    private bool _canActivateLever = false;
    private Collider2D _leverCollider;
    public bool CanClimb { get { return _canClimb; } }
    public bool CanActivate { get { return _canActivateLever;} set { _canActivateLever = value; } }
    public Collider2D Climbable { get { return _climbableCollider; } }
    public Collider2D LevelCollider { get { return _leverCollider; } }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable")) 
        {
            _canClimb = true;
            _climbableCollider = collision;
        }
        else if (collision.CompareTag("Lever"))
        {
            Debug.Log("colisionaste con palanca");
            _canActivateLever = true;
            _leverCollider = collision;
            Debug.Log("en colision");

        }

    }



    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable") || collision.CompareTag("Pushable"))
        {
            _canClimb = false;
            _climbableCollider = null;
        }
        else if (collision.CompareTag("Lever"))
        {
            Debug.Log("saliste de colision con palanca");
            _canActivateLever = false;
            _leverCollider = null;

        }
    }
}
