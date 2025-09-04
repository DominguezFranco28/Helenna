using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState :  IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private JumpDetector _jumpDetector;

    private bool subbed = false;

    private void OnSpecialAction()
    {
        _oldPlayerBehaviour.LastMovementInput = _oldPlayerBehaviour.MovementInput;
        _oldPlayerBehaviour.StopMovement(); //freno el movimiento al tirar el brazo
        _oldPlayerBehaviour.SetMovementEnabled(false); //deshabilito el movimiento al tirar el brazo
        _oldStateMachine.TransitionTo(_oldStateMachine.impulseState);
    }

    private void OnMove(Vector2 movement)
    {
        Debug.Log("MoveState - Move: " + movement + " - mag: " + movement.magnitude);

        _oldPlayerBehaviour.SetMovementInput(movement);
        if (movement.magnitude <= 0.01f)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }
    }

    public MoveState(OldPlayerBehaviour oldPlayerBehaviour, OldStateMachine oldStateMachine, JumpDetector jumpDetector)
    {
        this._oldPlayerBehaviour = oldPlayerBehaviour;
        this._oldStateMachine = oldStateMachine;
        this._jumpDetector = jumpDetector;
    }

    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;

                InputManager.Instance.SpecialActionPressed += OnSpecialAction;
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
            InputManager.Instance.SpecialActionPressed -= OnSpecialAction;
            InputManager.Instance.Move -= OnMove;
            subbed = false;
        }
    }

    public void Update()
    {
        //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //_oldPlayerBehaviour.SetMovementInput(input);
        /*if (input.magnitude <= 0.01f)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }*/
        
        if (_jumpDetector.CanJump)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.slideState);
        }
    }
}
