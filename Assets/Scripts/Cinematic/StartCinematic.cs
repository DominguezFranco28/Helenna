using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartCinematic : PlayerDetector
{
    [SerializeField] private CinematicController cinematicController;
    public override void Effect(Collider2D collision)
    {
        cinematicController.PlayCinematic();
        GameStateManager.Instance.SetState(GameState.Paused);
    }
}
