using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : PlayerDetector
{
    public override void Effect(Collider2D collision)
    {
        var playerHealth = collision.GetComponent<StatsManager>();
        if (playerHealth != null)
        {
            playerHealth.NewHealth(-3);
        }

    }
}
