using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileMoveState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private HoleDetector _holeDetector;
    private PlatformDetector _platformDetector;
    public AgileMoveState(AgilePlayerBehaviour agilePlayerBehaviour, AgileStateMachine agileStateMachine, PlatformDetector platformDetector)
    {
        this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;
        this._platformDetector = platformDetector;
        _holeDetector = agilePlayerBehaviour.HoleDetector;
    }
    public void Enter()
    {
        Debug.Log("You entered the state: AGILE MOVE");
        SFXManager.Instance.PlayLoop(_agilePlayerBehaviour.StepsSFX);
    }

    public void Exit()
    {
        Debug.Log("You left the state: AGILE MOVE");
        SFXManager.Instance.StopLoop();
    }
    
    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _agilePlayerBehaviour.SetMovementInput(input);
        if (input.magnitude <= 0.01f)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);
            return; //return para evitar que siga evaluando el resto de condiciones.
        }
        if (_holeDetector.CanDig == true)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.digState); 
            return;
        }
        if (_platformDetector.CanJump && Input.GetKeyDown(KeyCode.E))
        {

            _agilePlayerBehaviour.StopMovement();
            _agileStateMachine.TransitionTo(_agileStateMachine.jumpState);
            return;
        }

    }
}
