using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour

{
    private StatsManager statsManager;
    private int actualEnergy = 50000;
    private int maxEnergy = 50000;
    //Variables ligadas al observer //son realmente necesarias aca? o solo en el telekinesis
    void Start()
    {
        statsManager = GetComponent<StatsManager>(); //LLamo al StatsManager para que reciba actualizaciones.
                                                     //En este script, solo recibira de Energia.
    }
    public int Energy
    {
        get { return actualEnergy; } //Existe otra sintaxis mas moderna para el get;   get=>actualEnergy
        set
        {
            actualEnergy = Mathf.Clamp(value, 0, maxEnergy); //El clamp es una expresion para que limitar el valro ingresado entre 0 y el maximo. Luego lo guarda
            //El value toma un valro desde afuera, como hago en el script del recolecatble con el player.Energia.
            //No hace ninguna suma de manera directa. La suma (o resta, como voy a tener que hacer cuando se quemen los metales) se hace desde afuera.
            
            //TENGO QUE DISEÑAR UNA BARRA UI PARA MOSTRAR ESTA ENERGIA EN PANTALLA
            Debug.Log("Nueva energia" + actualEnergy);
        
        }
    }

}
