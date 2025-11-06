using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public void chargeScene(int scene)
    {
         SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Debug.Log("saliendo de la app");
        Application.Quit();
    }
}
