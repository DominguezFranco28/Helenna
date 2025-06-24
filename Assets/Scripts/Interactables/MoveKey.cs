using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveKey : PlayerDetector
{
    [SerializeField] private Transform moveSpawn;
    public override void Effect(Collider2D collision)
    {
        gameObject.transform.position = moveSpawn.transform.position;
    }
}
