using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MousePosition : MonoBehaviour
{
    private Vector2 _mouseWorldPos;
    private SpriteRenderer _sprite;
    public Vector2 MouseWorlPos
    {
        get { return (_mouseWorldPos - (Vector2)transform.position).normalized; }  //Propiedad publica para acceder desde otros scripts que necesiten ubicar el mouse en pantalla.
    }
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Vector3 rawMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Metodo que devuelve un vector3
        _mouseWorldPos = new Vector2(rawMousePos.x, rawMousePos.y); //Pero para mi logica 2d, ignoro la z con esta conversion explicita.
        FlipSprite();
    }
    private void FlipSprite()
    {

        //if (_mouseWorldPos.x < transform.position.x)
        //{
        //    _sprite.flipX = true; // Mirar a la izquierda
        //}
        //else
        //{
        //    _sprite.flipX = false; // Mirar a la derecha
        //}
    }
}
