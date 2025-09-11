using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevationSetter : MonoBehaviour
{
    public bool canTrigger = true;
    public event System.Action<ElevationSetter> OnTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger)
        {
            if (collision.tag.ToLower().Contains("player"))
            {
                CharacterVerticalCollider character = collision.gameObject.GetComponent<CharacterVerticalCollider>();
                if (character)
                {
                    OnTriggered?.Invoke(this);
                    character.toggle = true;
                }
            }
        }

    }

    public void DisableETrigger()
    {
        canTrigger = false;
    }
    public void EnableETrigger()
    {
        canTrigger = true;
    }
}
