using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CheckCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void OnTriggerEnter2D(Collider2D collision) { }
    public virtual void OnTriggerExit2D(Collider2D collision) { }


}
