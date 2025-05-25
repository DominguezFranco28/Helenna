using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Actionable : MonoBehaviour, IActiveable
{
    [SerializeField] private FranciscoPonce franciscoPrefab;
    [SerializeField] private GameObject interact;
    [SerializeField] private Transform spawnPoint;
    private bool isInstantiated = false;
  public void Activeable()
    {
        Debug.Log("ME ACTIVASTE!");
        gameObject.transform.rotation = Quaternion.Euler(0,0,160);
        Destroy(interact);
        interact = null;
        SpawnEnemy();
    }
    private void SpawnEnemy() 
    {
        if (!isInstantiated)
        {
           Instantiate(franciscoPrefab, spawnPoint.position, Quaternion.identity); //Boleana para asegurarme que solo puede instanciarse una vez al dar click a la Lever
           isInstantiated = true;
           
        }
    }
}
