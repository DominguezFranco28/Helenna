using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public event System.Action VictoryReached;
    public bool victory = false;

    public List<GameObject> characters = new List<GameObject>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!victory)
        {
            if (collision.tag.ToLower().Contains("player"))
            {
                GameObject character = collision.gameObject;
                if (!characters.Contains(character))
                {
                    characters.Add(character);
                    if (characters.Count == 3)
                    {
                        victory = true;
                        VictoryReached?.Invoke();
                    }
                }

            }
        }
        
    }

    private void ColliderBlip()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!victory)
        {
            if (collision.tag.ToLower().Contains("player"))
            {
                GameObject character = collision.gameObject;
                if (!characters.Contains(character))
                    characters.Remove(character);
            }
        }

    }
}
