using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextZone : PlayerDetector
{
    [SerializeField] private string _changeLevel;
    public override void Effect(Collider2D collision)
    {
        Debug.Log("saliendo de la zona");
        TransitionManager.Instance.ChangeLevel(_changeLevel);

    }
}
