using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RexTPHole : MonoBehaviour
{
    public GameObject exitHole;
    private GameObject dog = null;

    public SpriteRenderer entrySprite;
    public Color unusedColor;
    public Color usedColor;
    public SpriteRenderer exitSprite;

    private bool used = false;

    public ParticleSystem dirtParticles;

    private void Start()
    {
        if(entrySprite)
            entrySprite.color = unusedColor;
        if(exitSprite)
            exitSprite.enabled = false;

        dirtParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("dog"))
        {
            dog = collision.gameObject;
            dog.GetComponent<AgilePlayerBehaviour>().SetCurrentHole(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("dog"))
        {
            if (dog != null)
            {
                dog.GetComponent<AgilePlayerBehaviour>().ClearCurrentHole();
                dog = null;
            }
        }
    }

    public void Use()
    {
        if (!used)
        {
            if(entrySprite)
                entrySprite.color = usedColor;
            if(exitSprite)
                exitSprite.enabled = true;

            if(dirtParticles)
                dirtParticles.Play();
        }
        
    }
}
