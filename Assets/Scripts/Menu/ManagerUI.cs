using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class ManagerUI : MonoBehaviour, IObserver
{
    public TMP_Text energyText;
    public TMP_Text healthText;

    private void Start()
    {
        StatsManager.Instance.RegisterObserver(this); //Registro al ManagerUI como observador
    }
    //Metodo que recibe las notificaciones del sujeto
    public void OnNotify(int energy, int health)
    {
        energyText.text = "Energia disponible:  " + energy.ToString();
        healthText.text = "Vida: " + health.ToString();

    }



}
