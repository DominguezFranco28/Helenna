using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItemState : IState
{
    private OldStateMachine _stateMachine;
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private GrabObject _grabObject;

    private bool subbed = false;

    //private void InteractPressed()
    //{
    //    _oldPlayerBehaviour.StopMovement();
        
    //    if (_grabObject.PickedObject != null) // modif el input
    //    {
    //        //Drop/use object
    //        Debug.Log(_grabObject.PickedObject);
    //        _oldPlayerBehaviour.LowSpeed(false);
    //        _grabObject.DropItem();
    //        _stateMachine.TransitionTo(_stateMachine.idleState);

    //    }
    //    else
    //    {
    //        //Pickup
    //        _grabObject.GrabItem();
    //    }
    //}
    private void OnMove(Vector2 movement)
    {
        _oldPlayerBehaviour.SetMovementInput(movement);
        _oldPlayerBehaviour.LowSpeed(true);
    }

    public HoldItemState(OldPlayerBehaviour oldPlayerBehaviour, OldStateMachine oldStateMachine, GrabObject grabObject)
    {
        this._stateMachine = oldStateMachine;
        this._oldPlayerBehaviour = oldPlayerBehaviour;
        this._grabObject = grabObject;
    }
    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.Move += OnMove;
            }
        }
        
        Debug.Log("Entraste al estado ; HOLDITEM");
        
    }

    public void Exit()
    {
        
        Debug.Log("Saliste del estado ; HOLDITEM");
    }

    public void Update()
    {
        //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //_oldPlayerBehaviour.SetMovementInput(input);
        //_oldPlayerBehaviour.LowSpeed(true);
        
    }
}

//Debug.Log("aspretaste la R");
//_oldPlayerBehaviour.LowSpeed(false);
//_grabObject.PickedObject.transform.SetParent(null); //I set the parent to null, so it "drops" it
//_grabObject.PickedObject.GetComponent<Rigidbody2D>().simulated = true;
//_grabObject.ChangeSprite();
//if (_grabObject.OnPosition)
//{

//    _grabObject.PickedObject.tag = "Climbable"; //ESTO NECESARIO para que no te puedas trepar con la nena si la escalera esta en el pios
//    _grabObject.PickedObject.transform.position = _grabObject.OnPositionTransform;

//}
//_grabObject.DropItem();
//_stateMachine.TransitionTo(_stateMachine.idleState);