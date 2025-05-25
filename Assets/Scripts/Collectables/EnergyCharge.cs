using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCharge : PlayerDetector
{
    public override void Effect(Collider2D collision)
    {
        var player = collision.GetComponent<StatsManager>();
        Debug.Log("RECARGA DE ENERGIA");
        player.NewEnergy(100); 
        Destroy(gameObject, 0.2f);
    }

}
