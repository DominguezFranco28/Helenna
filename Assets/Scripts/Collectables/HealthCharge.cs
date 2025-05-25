using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCharge : PlayerDetector
{

    public override void Effect(Collider2D collision)
    {
        var player = collision.GetComponent<StatsManager>();
        Debug.Log("RECARGA DE VIDA");
        player.NewHealth(1);
        Destroy(gameObject, 0.2f);
    }

}
