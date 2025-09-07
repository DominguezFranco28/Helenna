using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileStateMachine
{
    private CharacterManager characterManager;

    public AgileMoveState moveState;
    public AgileIdleState idleState;
    public AgileDigState digState;
    public AgileJumpState jumpState;
    public AgileHoldItemState itemState;
    public IState CurrentState { get; private set; }

    public AgileStateMachine(AgilePlayerBehaviour player , PlatformDetector platformDetector , GrabObject grabObject)
    {
        this.moveState = new AgileMoveState(player, this);
        this.idleState = new AgileIdleState(player, this);
        this.digState = new AgileDigState(player, this);
        this.jumpState = new AgileJumpState(player, this, platformDetector);
        this.itemState = new AgileHoldItemState (player, this, grabObject);
    }

    public void Initialize(IState startingState)
    {
        InitStates(startingState);
        CurrentState = startingState;

        characterManager = CharacterManager.Instance;
    }

    public void TransitionTo(IState nextState)
    {
        if (characterManager)
        {
            string characterName = "DogPlayer";
            if (characterManager.GetActiveCharacter() != characterName) return;
        }

        if (nextState != CurrentState)
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
        IState[] states = { idleState, moveState, digState, jumpState, itemState};
        foreach (IState state in states)
        {
            TransitionTo(state);
        }
        TransitionTo(startingState);
    }
}
