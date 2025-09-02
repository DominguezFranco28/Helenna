 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldStateMachine

//Notes for me,to keep in mind when creating state machines in Unity 

// The unity e-book clarifies that the StateMachine doesn't necessarily inherit from MonoBehaviour.
// This is because it's a design pattern not exclusively for Unity.
// Furthermore, it aligns with good practices regarding Separation of Concerns,
// improves organization, and keeps the pattern clean and reusable in various scenarios.

// Since it doesn't inherit from MonoBehaviour, it's necessary to use constructors to set up each instance of the desired states.
// GetComponent cannot be used because it's a method inherited from MonoBehaviour.

{
    public MoveState moveState;
    public IdleState idleState;
    public ImpulseState impulseState;
    public JumpState jumpState;
    public HoldItemState holdItemState;
    public IState CurrentState { get; private set; } //Read-only. External object can set the Initialize method to establish a default state
    public OldStateMachine(OldPlayerBehaviour oldPlayer, JumpDetector jumpDetector, GrabObject grabObject)
    {
        this.moveState = new MoveState(oldPlayer, this, jumpDetector);
        this.idleState = new IdleState(oldPlayer, this);
        this.impulseState = new ImpulseState(oldPlayer, this);
        this.jumpState = new JumpState(oldPlayer, this, jumpDetector);
        this.holdItemState = new HoldItemState(oldPlayer, this, grabObject);

        //It was necessary to add the "this".
        //I pass this instantiation of the StateMachine class as
        //a parameter so that all states know the ONLY StateMachine of existing states,
        //and do not build new ones for me on each instantiation.
    }


    //Enter, Update and Exit methods of the Istate interface, to manage the entry and exit of states.
    public void Initialize(IState startingState)
    {
        InitStates(startingState);
        CurrentState = startingState;
    }
    public void TransitionTo (IState nextState)
    {
        if (nextState != CurrentState)
        {
            if (CurrentState != null) CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();
        }

    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    public void InitStates(IState startingState)
    {
        //cicla por todos los estados para subscribir todos los inputs
        IState[] states = {idleState, moveState, impulseState, jumpState, holdItemState};
        foreach(IState state in states)
        {
            TransitionTo(state);
        }
        TransitionTo(startingState);
    }
}
