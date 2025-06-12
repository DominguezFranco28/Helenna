using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileStateMachine
{
    public AgileMoveState moveState;
    public AgileIdleState idleState;
    public AgileDigState digState;


    public IState CurrentState { get; private set; }

    public AgileStateMachine(AgilePlayerBehaviour player)
    {
        this.moveState = new AgileMoveState(player, this);
        this.idleState = new AgileIdleState(player, this);
        this.digState = new AgileDigState(player, this);
    }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
