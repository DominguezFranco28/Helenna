using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveKey : PlayerDetector
{
    [SerializeField] private Transform _moveSpawn;
    public override void Effect(Collider2D collision)
    {
        gameObject.transform.position = _moveSpawn.transform.position;
    }
}
