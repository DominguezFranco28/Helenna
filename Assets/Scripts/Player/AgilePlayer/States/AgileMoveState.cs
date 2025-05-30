using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileMoveState : IState
{
    private AgilePlayerBehaviour _playerBehaviour;
    private AgileStateMachine _agileStateMachine;
    public AgileMoveState(AgilePlayerBehaviour playerBehaviour, AgileStateMachine agileStateMachine)
    {
        this._playerBehaviour = playerBehaviour;
        this._agileStateMachine = agileStateMachine;
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
