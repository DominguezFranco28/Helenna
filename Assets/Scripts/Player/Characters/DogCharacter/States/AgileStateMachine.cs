using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileStateMachine
{
    public AgileMoveState moveState;
    public AgileIdleState idleState;
    public AgileDigState digState;
    public AgileJumpState jumpState;
    public IState CurrentState { get; private set; }

    public AgileStateMachine(AgilePlayerBehaviour player , PlatformDetector platformDetector)
    {
        this.moveState = new AgileMoveState(player, this, platformDetector);
        this.idleState = new AgileIdleState(player, this);
        this.digState = new AgileDigState(player, this);
        this.jumpState = new AgileJumpState(player, this, platformDetector);
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
