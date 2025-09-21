using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState :  IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private JumpDetector _jumpDetector;
    private GrabObject _grabObject;
    private bool interacting = false;
    private bool subbed = false;
    public MoveState(OldPlayerBehaviour oldPlayerBehaviour, OldStateMachine oldStateMachine, JumpDetector jumpDetector, GrabObject grabObject)
    {
        this._oldPlayerBehaviour = oldPlayerBehaviour;
        this._oldStateMachine = oldStateMachine;
        this._jumpDetector = jumpDetector;
       this._grabObject = grabObject;
    }

    private void OnSpecialAction()
    {
        _oldPlayerBehaviour.LastMovementInput = _oldPlayerBehaviour.MovementInput;
        _oldPlayerBehaviour.StopMovement(); //freno el movimiento al tirar el brazo
        _oldPlayerBehaviour.SetMovementEnabled(false); //deshabilito el movimiento al tirar el brazo
        _oldStateMachine.TransitionTo(_oldStateMachine.impulseState);
    }
    private void OnSpecialActionHeld()
    {
        _oldPlayerBehaviour.LastMovementInput = _oldPlayerBehaviour.MovementInput;
        _oldPlayerBehaviour.StopMovement(); //freno el movimiento al tirar el brazo
        _oldPlayerBehaviour.SetMovementEnabled(false); //deshabilito el movimiento al tirar el brazo
        _oldPlayerBehaviour.SwitchArmType(); //swithceamos el type del disparo del viejo (entre pull y push)
        Debug.Log("Switched Arm Type to: " + _oldPlayerBehaviour.GetCurrentArmType());
    }

    private void OnMove(Vector2 movement)
    {
        _oldPlayerBehaviour.SetMovementInput(movement);
        if (movement.magnitude <= 0.01f)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }
        if (_grabObject.CanGrabDog && interacting)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.throwState);
        }
    }
    private void InteractPressed()
    {

        //TEST FUNCIONANMIENTO SWITCH MANO
        //_oldPlayerBehaviour.SwitchArmType(); //swithceamos el type del disparo del viejo (entre pull y push)
        Debug.Log("Switched Arm Type to: " + _oldPlayerBehaviour.GetCurrentArmType());



        interacting = true;
        Debug.Log("Interacting: " + interacting);
        InputManager.Instance.InvokeAction(() => interacting = false, 0.5f);
    }


    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;

             //   InputManager.Instance.SpecialActionHeld += OnSpecialActionHeld; //no funciona como deberia
                InputManager.Instance.SpecialActionPressed += OnSpecialAction;
                InputManager.Instance.InteractPressed += InteractPressed;
                InputManager.Instance.Move += OnMove;
            }
        }
        Debug.Log("You entered the state: OLD MOVE");
        _oldPlayerBehaviour.SetMovementEnabled(true); //deshabilito el movimiento al tirar el brazo
        SFXManager.Instance.PlayLoop(_oldPlayerBehaviour.StepsSFX);
    }

    public void Exit()
    {
        Debug.Log("You left the state: OLD MOVE");
        SFXManager.Instance.StopLoop();

        //desuscripcion del estado move porque daba problema con el impulse. Seguia interceptando inputs cuando no le corresponida por mas que harold haya ejecutado la accion de disparo, buigeaba anims
        //Seguia interceptando inputs cuando no le corresponida por mas que harold haya ejecutado la accion de disparo, buigeaba anims
        if (InputManager.Instance != null)
        {
            subbed = false;
           // InputManager.Instance.SpecialActionHeld -= OnSpecialActionHeld;
            InputManager.Instance.SpecialActionPressed -= OnSpecialAction;
            InputManager.Instance.InteractPressed -= InteractPressed;
            InputManager.Instance.Move -= OnMove;
        }
    }

    public void Update()
    {
        if (_jumpDetector.CanJump)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.slideState);
        }
    }
}
