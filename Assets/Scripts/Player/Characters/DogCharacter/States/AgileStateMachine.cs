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
    public AgileThrownState thrownState;
    public AgilePulledState pulledState;
    public IState CurrentState { get; private set; }

    public AgileStateMachine(AgilePlayerBehaviour player , GrabObject grabObject, AgilePlayerController agilePlayerController)
    {
        this.moveState = new AgileMoveState(player, this);
        this.idleState = new AgileIdleState(player, this);
        this.digState = new AgileDigState(player, this);
        this.jumpState = new AgileJumpState(player, this);
        this.itemState = new AgileHoldItemState (player, this, grabObject);
        this.thrownState = new AgileThrownState(player, this, agilePlayerController);
        this.pulledState = new AgilePulledState(player, this, agilePlayerController);
    }

    public void Initialize(IState startingState)
    {
        InitStates(startingState);
        CurrentState = startingState;

        characterManager = CharacterManager.Instance;
    }

    public void TransitionTo(IState nextState, bool forceTransition = false)
    {
        //forceTransition = true ignora la comprobacion de si rex esta activo, PARA PODER forzarlo desde el controlador del perro cuando lo agarra harold
        if (!forceTransition && characterManager != null)
        {
            string characterName = "DogPlayer";
            if (characterManager.GetActiveCharacter() != characterName) return;
        }//aca se bloquea el debug de todos los estados, no deja actuar a la maquina de estados

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
        //cicla por todos los estados necesarios para subscribir todos los inputs
        IState[] states = { idleState, moveState}; 
        foreach (IState state in states)
        {
            TransitionTo(state);
        }
        TransitionTo(startingState);
    }
}
