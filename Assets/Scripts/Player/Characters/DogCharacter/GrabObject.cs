using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [SerializeField] private GameObject _mouth;
    private GameObject _pickedObject = null; 

    void Update()
    {
        if (_pickedObject != null && Input.GetKey("r"))
        {
            _pickedObject.gameObject.transform.SetParent(null); //I set the parent to null, so it "drops" it
            _pickedObject.GetComponent<Rigidbody2D>().simulated = true;
            _pickedObject = null;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickableObject") && Input.GetKey("e"))
        {
            Debug.Log("You pressed E");
            if (_pickedObject == null)
            {
                collision.transform.position = _mouth.transform.position;
                collision.gameObject.transform.SetParent(_mouth.gameObject.transform); //set the parent so follow de mouth 
                collision.GetComponent<Rigidbody2D>().simulated = false;
                _pickedObject = collision.gameObject;
            }
        }
    }
}
