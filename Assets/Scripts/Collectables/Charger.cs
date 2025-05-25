using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : PlayerDetector
{
    [SerializeField] private GameObject chargeBG;
    private StatsManager stats;
    public override void Effect(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerEnergy>();
        stats = collision.GetComponent<StatsManager>();
        if (collision == true && player.Energy < 500)
        {
        InvokeRepeating("Recharge", 0.3f, 0.3f);
            
        }
    }
    private void Recharge()
    {
        chargeBG.SetActive(true); 
        Debug.Log("RECARGANDO  ENERGIA");
        stats.NewEnergy(50);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke("Recharge");
        chargeBG.SetActive(false);
    }

}
