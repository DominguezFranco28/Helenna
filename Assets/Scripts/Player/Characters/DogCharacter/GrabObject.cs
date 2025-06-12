using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [SerializeField] private GameObject _mouth;
    private GameObject _pickedObject = null; 

    // Tengo que integrarlo a la stateMachine
    void Update()
    {
        if (_pickedObject != null && Input.GetKey("r"))
        {
            _pickedObject.gameObject.transform.SetParent(null); //Seteo el parent en null, asi que lo "suelta"
            _pickedObject.GetComponent<Rigidbody2D>().simulated = true;
            _pickedObject = null;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickableObject") && Input.GetKey("e"))
        {
            Debug.Log("apretaste la E");
            if (_pickedObject == null)
            {
                collision.transform.position = _mouth.transform.position;
                collision.gameObject.transform.SetParent(_mouth.gameObject.transform); //Lo seteo hijo de la Boca del perro.
                collision.GetComponent<Rigidbody2D>().simulated = false;
                _pickedObject = collision.gameObject;
            }
        }
    }
}
