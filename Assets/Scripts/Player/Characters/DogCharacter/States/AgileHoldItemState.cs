using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileHoldItemState : IState

{
    private AgileStateMachine _stateMachine;
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private GrabObject _grabObject;

    private bool interacting = false;
    private bool doingAction = false;

    private bool subbed = false;

    private void OnMove(Vector2 movement)
    {
        _agilePlayerBehaviour.SetMovementInput(movement);
    }
    private void InteractPressed()
    {
        interacting = true;
        if (InputManager.Instance != null)
            InputManager.Instance.InvokeAction(() => interacting = false, 0.1f);
    }
    private void ActionPressed()
    {
        doingAction = true;
        if (InputManager.Instance != null)
            InputManager.Instance.InvokeAction(() => doingAction = false, 0.1f);
    }

    public AgileHoldItemState(AgilePlayerBehaviour agilePlayerBehaviour, AgileStateMachine agileStateMachine, GrabObject grabObject)
    {
        this._stateMachine = agileStateMachine;
        this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._grabObject = grabObject;
    }
    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;
                InputManager.Instance.InteractPressed += InteractPressed;
                InputManager.Instance.ActionPressed += ActionPressed;
                InputManager.Instance.Move += OnMove;
            }
        }
        

        Debug.Log("Entraste al estado ; AGILE HOLD ITEM");
        _agilePlayerBehaviour.StopMovement();
        _grabObject.GrabItem();
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado ; AGILE HOLD ITEM");
    }

    public void Update()
    {
        /*Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _agilePlayerBehaviour.SetMovementInput(input);*/
        if (_grabObject.PickedObject != null)
        {
            if (interacting)
            {
                  _grabObject.DropItem();
                  _stateMachine.TransitionTo(_stateMachine.moveState);

            }
            if (doingAction && _agilePlayerBehaviour.CanJump && _agilePlayerBehaviour.DelayCompleted)
            {
                _stateMachine.jumpState.Object(_grabObject.PickedObject);
                _stateMachine.TransitionTo(_stateMachine.jumpState);

                

            }
            if (_agilePlayerBehaviour.CanDig)
            {
                 _stateMachine.digState.Object(_grabObject.PickedObject);
                _stateMachine.TransitionTo(_stateMachine.digState);

            }
        }
    }
}
