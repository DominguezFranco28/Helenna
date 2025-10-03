using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineZoneDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("old"))
        {
            OldPlayerBehaviour character = collision.gameObject.GetComponent<OldPlayerBehaviour>();
            if (character)
            {
                character.UnlockZipline = true;
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
                character.UnlockZipline = false;
            }
        }
    }
}
    
    


