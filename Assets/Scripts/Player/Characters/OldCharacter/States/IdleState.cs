using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState :  IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private TriggerDetector _triggerDetector;

    private bool subbed = false;
    private bool interacting = false;

    private void OnSpecialAction()
    {
        if(!_oldPlayerBehaviour.IsInControll) return; 
        //_oldPlayerBehaviour.SwitchArmType(false);
        _oldPlayerBehaviour.SetMovementEnabled(false); //deshabilito el movimiento al tirar el brazo
        _oldPlayerBehaviour.StopMovement(); //freno el movimiento al tirar el brazo
        _oldPlayerBehaviour.SwitchArmType(true);
        _oldStateMachine.TransitionTo(_oldStateMachine.impulseState);
    }
    private void OnMove(Vector2 movement)
    {
        if (!_oldPlayerBehaviour.IsInControll) return;
        if (movement.magnitude > 0.01f)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.moveState);
            _oldPlayerBehaviour.SetMovementInput(movement);
        }
    }

    //Constructor, because it does not inherit from monobehaviour
    public IdleState(OldPlayerBehaviour oldPlayerBehaviour, OldStateMachine oldStateMachine, TriggerDetector triggerDetector) 
    {
        this._oldPlayerBehaviour = oldPlayerBehaviour;
        this._oldStateMachine = oldStateMachine;
        this._triggerDetector = triggerDetector;
    }
    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;
                InputManager.Instance.ActionPressed += OnAction;
                InputManager.Instance.SpecialActionPressed += OnSpecialAction;
                InputManager.Instance.InteractPressed += InteractPressed;
                InputManager.Instance.Move += OnMove;
            }
        }
        
        Debug.Log("You entered the state: OLD IDLE");
        if (_oldPlayerBehaviour)
        {
            _oldPlayerBehaviour.SetMovementEnabled(true);
            _oldPlayerBehaviour.SetMovementInput(Vector2.zero);
        }
        
    }
    public void Exit()
    {
        Debug.Log("You left the state: OLD IDLE");
        //desuscripcion del estado move porque daba problema con el impulse.
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ActionPressed -= OnAction;
            InputManager.Instance.SpecialActionPressed -= OnSpecialAction;
            InputManager.Instance.InteractPressed -= InteractPressed;
            InputManager.Instance.Move -= OnMove;
            subbed = false;
        }
    }

    private void InteractPressed()
    {
        if (!_oldPlayerBehaviour.IsInControll) return;
        interacting = true;
        if (_triggerDetector.CanActivate && interacting)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.actionState);
            return;
        }

        _oldPlayerBehaviour.SetMovementEnabled(false); //deshabilito el movimiento al tirar el brazo
        _oldPlayerBehaviour.StopMovement(); //freno el movimiento al tirar el brazo
        _oldPlayerBehaviour.SwitchArmType(false); //swithceamos el type del disparo del viejo (entre pull y push)
        _oldStateMachine.TransitionTo(_oldStateMachine.impulseState);

        if (InputManager.Instance != null)
            InputManager.Instance.InvokeAction(() => interacting = false, 0.1f);
        //TEST FUNCIONANMIENTO SWITCH MANO
        //_oldPlayerBehaviour.SwitchArmType(); //swithceamos el type del disparo del viejo (entre pull y push)
        //Debug.Log("Switched Arm Type to: " + _oldPlayerBehaviour.GetCurrentArmType());
        //interacting = true;
        //Debug.Log("Interacting: " + interacting);
        //InputManager.Instance.InvokeAction(() => interacting = false, 0.5f);

    }
    private void OnAction()

    {
        if (!_oldPlayerBehaviour.IsInControll) return;

        // uso el LastCardinalInput porque guarda la ultima direc de movimiento (no diagonal),
        // y evito que sea el vectorzero de stopmovoement
        Vector2 throwDirection = _oldPlayerBehaviour.LastCardinalInput;

        // Si LastCardinalInput es (0,0) (x ser el incio de juego o algun otro metodo-evento), usa una direc por defecto.
        if (throwDirection == Vector2.zero)
            throwDirection = Vector2.down;
        

        _oldPlayerBehaviour.LastMovementInput = throwDirection; // Guarda la dirección de lanzamiento

        if (_triggerDetector.CanGrabDog && _oldPlayerBehaviour.UnlockThrow)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.throwState);
        }
    }
    public void Update()
    {

    }
}

