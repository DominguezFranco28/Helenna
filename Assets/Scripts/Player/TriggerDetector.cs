using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    private bool _canJump = false;
    private bool _canActivateLever = false;
    private bool _canGrabDog = false;
    private Collider2D _leverCollider;
    private GameObject _pickedRex = null;
    public Collider2D LevelCollider { get { return _leverCollider; } }
    public bool CanActivate { get { return _canActivateLever; } set { _canActivateLever = value; } }
    public bool CanJump { get { return _canJump; } set { _canJump = value; } }
    public bool CanGrabDog { get { return _canGrabDog; } }

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
        if (collision.gameObject.CompareTag("Throwable"))
        {
            _canGrabDog = true;
            //Debug.Log("Can grab dog " + CanGrabDog);
            _pickedRex = collision.gameObject;
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
        if (collision.gameObject.CompareTag("Throwable"))
        {
            _canGrabDog = false;
            _pickedRex = null;

        }
    }
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Collider2D>().isTrigger = true;
    }
}
