using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHabilities : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("old"))
        {
            OldPlayerBehaviour character = collision.gameObject.GetComponent<OldPlayerBehaviour>();
            if (character)
            {
                character.UnlockThrow = false;
            }
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("old"))
        {
            OldPlayerBehaviour character = collision.gameObject.GetComponent<OldPlayerBehaviour>();
            if (character)
            {
                character.UnlockThrow = true;
            }
        }
    }
}
