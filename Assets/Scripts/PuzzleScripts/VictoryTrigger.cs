using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public event System.Action VictoryReached;
    public bool victory = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("old"))
        {
            if (!victory)
            {
                victory = true;
                VictoryReached?.Invoke();
            }
            
        }
    }
}
