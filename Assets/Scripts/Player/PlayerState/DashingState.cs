using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState :  IState
{
    private PlayerBehaviour _playerMovement;
    private StateMachine _stateMachine;
    public DashingState(PlayerBehaviour player, StateMachine stateMachine)
    {
        this._playerMovement = player;
        _stateMachine = stateMachine;
    }
    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
