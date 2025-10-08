using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildStateMachine
{
    private CharacterManager characterManager;

    public ChildMoveState moveState;
    public ChildIdleState idleState;
    public ChildClimbState climbState;
    public ChildActionState actionState;
    public ChildZiplineState ziplineState;
    public IState CurrentState { get; private set; }
    public ChildStateMachine(ChildPlayerBehaviour player)
    {
        this.moveState = new ChildMoveState(player, this);
        this.idleState = new ChildIdleState(player, this);
        //Los detectores en nina los pase de forma diferente haciendo uso de un Enum para poder tener diferentes colliders para cada tipo de deteccionn deseada
        //En el behaviour los asignna, recorriendo a sus GO hijos para cada tipo de detecteccionn deseada
        this.climbState = new ChildClimbState(player, this, player.ClimbDetector);
        this.actionState = new ChildActionState (player, this, player.LeverDetector);
        this.ziplineState = new ChildZiplineState(player, this, player.ZiplineDetector);
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
            string characterName = "ChildPlayer";
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
        IState[] states = {idleState, moveState};
        foreach (IState state in states)
        {
            TransitionTo(state);
        }
        TransitionTo(startingState);
    }
}
