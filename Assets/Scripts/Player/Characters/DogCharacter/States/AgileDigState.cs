using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileDigState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private HoleDetector _holeDetector;

    private float _digDelay = 0.3f; 
    private float _digTimer;
    private bool _delayCompleted;
    public AgileDigState(AgilePlayerBehaviour agilePlayerBehaviour, AgileStateMachine agileStateMachine)
    {
        this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;
        this._holeDetector = agilePlayerBehaviour.HoleDetector;

    }
    public void Enter()
    {
        Debug.Log("You entered the state: DIG");
        _agilePlayerBehaviour.Animator.SetBool("Dig", true); 
        _digTimer = 0f;
        _delayCompleted = false;
        SFXManager.Instance.PlaySFX(_agilePlayerBehaviour.DigSFXClip);
    }

    public void Exit()
    {
        Debug.Log("You left the state: DIG");
        if (_holeDetector.CanDig == false)
        {
            _agilePlayerBehaviour.Animator.SetBool("Dig", false);
        }
    }

    public void Update()
    {
        // Wait for delay
        if (!_delayCompleted)
        {
            _digTimer += Time.deltaTime;
            if (_digTimer >= _digDelay)
            {
                _delayCompleted = true;
                Debug.Log("End of delay");

            }
            return; // skip the update until delay is over
        }

        Vector2 input = new Vector2(0, Input.GetAxisRaw("Vertical")); //set the horizontal move to 0
        _agilePlayerBehaviour.SetMovementInput(input);
        // If we leave the gap we go to idle
        if (!_holeDetector.CanDig)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);
        }
    }
}
