using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MousePosition : MonoBehaviour
{
    private Vector2 _mouseWorldPos;
    private SpriteRenderer _sprite; // use in FlipSprite

    
    //Public property to access from other scripts that need to position the mouse on the screen.
    public Vector2 MouseWorlPos
    {
        get { return (_mouseWorldPos - (Vector2)transform.position).normalized; }
    }
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Vector3 rawMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //return a vector3
        _mouseWorldPos = new Vector2(rawMousePos.x, rawMousePos.y); //But for my 2d logic, I ignore the z with this explicit conversion.
        //FlipSprite();
    }
    //private void FlipSprite()
    //{

    //    //if (_mouseWorldPos.x < transform.position.x)
    //    //{
    //    //    _sprite.flipX = true; // Mirar a la izquierda
    //    //}
    //    //else
    //    //{
    //    //    _sprite.flipX = false; // Mirar a la derecha
    //    //}
    //}
}
