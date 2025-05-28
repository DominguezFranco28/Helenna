using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Actionable : MonoBehaviour, IActiveable
{
    [SerializeField] private GameObject interact;
    [SerializeField] private Transform spawnPoint;
    private bool isInstantiated = false;
  public void Activeable()
    {
        Debug.Log("ME ACTIVASTE!");
        gameObject.transform.rotation = Quaternion.Euler(0,0,160);
        Destroy(interact);
    }
}
