using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PlayerDetector
{
    //Sobrescritura del metodo obligatorio heredado de la clase Collectable. Diferente efecto para cada subclase.
    public override void Effect(Collider2D collision)
    {
        var playerPower = collision.GetComponent<StatsManager>();
        Debug.Log("Potenciador de ataque ACTIVADO");
        playerPower.UsePowerUp(25);
        Destroy(gameObject, 0.2f);
    }
    
}
