using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDetector : MonoBehaviour
// ABSTRACT CLASS. As a template for other collectibles 
//Abstract class and interface are two ways of giving abstraction to something, not really reason for this here, i just want to try it
{
    public virtual void OnTriggerEnter2D(Collider2D collision)
    //I establish a "common calling" for daughters, to come into contact with the Player
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Effect(collision);            
        }
    }
    public abstract void Effect(Collider2D collision); //Signature of the method, mandatory for her daughters.
}
