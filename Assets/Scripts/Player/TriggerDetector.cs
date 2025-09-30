using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    private bool _canJump = false;
    private bool _canActivateLever = false;
    private Collider2D _leverCollider;
    public Collider2D LevelCollider { get { return _leverCollider; } }
    public bool CanActivate { get { return _canActivateLever; } set { _canActivateLever = value; } }
    public bool CanJump { get { return _canJump; } set { _canJump = value; } }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slidable"))
        {
            CanJump = true;
        }
        if (collision.CompareTag("Lever"))
        {
            Debug.Log("colisionaste con palanca");
            _canActivateLever = true;
            _leverCollider = collision;
            Debug.Log("en colision");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slidable"))
        {
            CanJump = false;
        }
        if (collision.CompareTag("Lever"))
        {
            Debug.Log("saliste de colision con palanca");
            _canActivateLever = false;
            _leverCollider = null;

        }
    }
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Collider2D>().isTrigger = true;
    }
}
