using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RexTPHole : MonoBehaviour
{
    public GameObject exitHole;
    private GameObject dog = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("dog"))
        {

            dog = collision.gameObject;
            dog.GetComponent<AgilePlayerBehaviour>().SetCurrentHole(exitHole.transform);
            Debug.Log("Rex entered hole, exit at: " + exitHole.transform.position);
            //StartCoroutine(DigWait());
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
    //private IEnumerator DigWait()
    //{
    //    yield return new WaitForSeconds(1f);
    //    dog.GetComponent<AgilePlayerBehaviour>().SetMovementEnabled(true);
    //    dog.gameObject.transform.position = exitHole.transform.position;
    //    dog = null;
    //}
}
