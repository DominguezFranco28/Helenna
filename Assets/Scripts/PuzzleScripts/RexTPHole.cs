using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RexTPHole : MonoBehaviour
{
    public GameObject exitHole;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("dog"))
        {
            collision.gameObject.transform.position = exitHole.transform.position;
        }
    }
}
