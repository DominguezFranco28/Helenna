using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileIdleState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    public AgileIdleState(AgilePlayerBehaviour playerBehaviour, AgileStateMachine agileStateMachine )
    {
        this._agilePlayerBehaviour = playerBehaviour;
        this._agileStateMachine = agileStateMachine;
    }
    public void Enter()
    {
        Debug.Log("Hola soy el pj2");
        Debug.Log("Hola soy el pj2");
       
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        Debug.Log("Hola soy el pj2, UPDATE");
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _agilePlayerBehaviour.SetMovementInput(input);
    }
}
