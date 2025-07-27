using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItemState : IState

{
    private OldStateMachine _stateMachine;
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private GrabObject _grabObject;
    public HoldItemState(OldPlayerBehaviour oldPlayerBehaviour, OldStateMachine oldStateMachine, GrabObject grabObject)
    {
        this._stateMachine = oldStateMachine;
        this._oldPlayerBehaviour = oldPlayerBehaviour;
        this._grabObject = grabObject;
    }
    public void Enter()
    {
        Debug.Log("Entraste al estado ; HOLDITEM");
        _oldPlayerBehaviour.StopMovement();
        _grabObject.GrabItem();
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado ; HOLDITEM");
    }

    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _oldPlayerBehaviour.SetMovementInput(input);
        _oldPlayerBehaviour.LowSpeed(true);
        if (_grabObject.PickedObject != null && Input.GetKeyDown(KeyCode.R)) // modif el input
        {
            _oldPlayerBehaviour.LowSpeed(false);
            _grabObject.ChangeSprite();
            _grabObject.PickedObject.transform.SetParent(null); //I set the parent to null, so it "drops" it
            _grabObject.PickedObject.GetComponent<Rigidbody2D>().simulated = true;
            if (_grabObject.OnPosition)
            {
                
                _grabObject.PickedObject.tag = "Climbable"; //ESTO NECESARIO para que no te puedas trepar con la nena si la escalera esta en el pios
                _grabObject.PickedObject.transform.position = _grabObject.OnPositionTransform;
            }
            _grabObject.PickedObject = null;
            _stateMachine.TransitionTo(_stateMachine.idleState);

        }
    }
}
