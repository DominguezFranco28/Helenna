using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildStateMachine
{
    public ChildMoveState moveState;
    public ChildIdleState idleState;
    public ChildClimbState climbState;


    public IState CurrentState { get; private set; }

    public ChildStateMachine(ChildPlayerBehaviour player, ClimbDetector climbDetector)
    {
        this.moveState = new ChildMoveState(player, this);
        this.idleState = new ChildIdleState(player, this);
        this.climbState = new ChildClimbState(player, this, climbDetector);
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
