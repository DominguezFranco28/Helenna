using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealZone : PlayerDetector
{
    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void Effect(Collider2D collision)
    {
        _spriteRenderer.color = new Color (0,0,0,0);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DogPlayer"))
        {

        _spriteRenderer.color = new Color (0,0,0,100);
        }
    }
}
