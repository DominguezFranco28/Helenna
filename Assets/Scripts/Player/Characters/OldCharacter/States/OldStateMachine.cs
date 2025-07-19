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
    public IState CurrentState { get; private set; } //Read-only. External object can set the Initialize method to establish a default state
    public OldStateMachine(OldPlayerBehaviour oldPlayer, JumpDetector jumpDetector)
    {
        this.moveState = new MoveState(oldPlayer, this, jumpDetector);
        this.idleState = new IdleState(oldPlayer, this);
        this.impulseState = new ImpulseState(oldPlayer, this);
        this.jumpState = new JumpState(oldPlayer, this, jumpDetector);

        //It was necessary to add the "this".
        //I pass this instantiation of the StateMachine class as
        //a parameter so that all states know the ONLY StateMachine of existing states,
        //and do not build new ones for me on each instantiation.
    }


    //Enter, Update and Exit methods of the Istate interface, to manage the entry and exit of states.
    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }
    public void TransitionTo (IState nextState)
    {
        CurrentState.Exit(); 
        CurrentState= nextState;
        nextState.Enter();

    }
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
