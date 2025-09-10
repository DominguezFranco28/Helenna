using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerDetector : MonoBehaviour
{
    [SerializeField] private bool _canClimb = false;
    [SerializeField] private bool _canUseZipline = false;
    private Collider2D _climbableCollider;

    private bool _canActivateLever = false;
    private Collider2D _leverCollider;
    public bool CanClimb { get { return _canClimb; } }
    public bool CanUseZipline { get { return _canUseZipline; }set { _canUseZipline = value; } }
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
        
        if (collision.CompareTag("Lever"))
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
        
        if (collision.CompareTag("Lever"))
        {
            Debug.Log("saliste de colision con palanca");
            _canActivateLever = false;
            _leverCollider = null;

        }
        if (collision.CompareTag("Zipline"))
        {
            Debug.Log("saliste de colision con tirolesa");
            _canUseZipline = false;
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Zipline"))
        {
            _canUseZipline = true;
        }
    }
    public void ResetZipline()
    {
        _canUseZipline = false;
    }
}
