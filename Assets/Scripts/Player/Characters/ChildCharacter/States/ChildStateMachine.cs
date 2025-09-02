using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildStateMachine
{
    public ChildMoveState moveState;
    public ChildIdleState idleState;
    public ChildClimbState climbState;
    public ChildActionState actionState;
    public IState CurrentState { get; private set; }
    public ChildStateMachine(ChildPlayerBehaviour player, ChildTriggerDetector actionDetector)
    {
        this.moveState = new ChildMoveState(player, this, actionDetector);
        this.idleState = new ChildIdleState(player, this);
        this.climbState = new ChildClimbState(player, this, actionDetector);
        this.actionState = new ChildActionState (player, this, actionDetector);
    }
    public void Initialize(IState startingState)
    {
        InitStates(startingState);
        CurrentState = startingState;
    }
    public void TransitionTo(IState nextState)
    {
        if(nextState != CurrentState)
        {
            if (CurrentState != null) CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();
        }
    }
    public void Update()
    {
        CurrentState?.Update();
    }

    public void InitStates(IState startingState)
    {
        //cicla por todos los estados para subscribir todos los inputs
        IState[] states = { idleState, moveState, climbState, actionState};
        foreach (IState state in states)
        {
            TransitionTo(state);
        }
        TransitionTo(startingState);
    }
}
