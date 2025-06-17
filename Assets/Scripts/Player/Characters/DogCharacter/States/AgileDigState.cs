using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileDigState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private HoleDetector _holeDetector;

    private float _digDelay = 0.3f; // segundos de espera antes de aplicar lógica de dig
    private float _digTimer;
    private bool _delayCompleted;
    public AgileDigState(AgilePlayerBehaviour playerBehaviour, AgileStateMachine agileStateMachine)
    {
        this._agilePlayerBehaviour = playerBehaviour;
        this._agileStateMachine = agileStateMachine;
        this._holeDetector = playerBehaviour.HoleDetector;

    }
    public void Enter()
    {
        Debug.Log("Entraste al estado : DIG");
        _agilePlayerBehaviour.Animator.SetBool("Dig", true); 
        _digTimer = 0f;
        _delayCompleted = false;
        SFXManager.Instance.PlaySFX(_agilePlayerBehaviour.DigSFXClip);


    }

    public void Exit()
    {
        Debug.Log("saliste del estado: DIG");


        if (_holeDetector.canDig == false)
        {
            _agilePlayerBehaviour.Animator.SetBool("Dig", false);
        }

    }

    public void Update()
    {
        // Esperrar a que pase el delay
        if (!_delayCompleted)
        {
            _digTimer += Time.deltaTime;
            if (_digTimer >= _digDelay)
            {
                _delayCompleted = true;
                Debug.Log("Fin del delay, empieza la lógica del DigState");

            }
            return; // mientras no termine el delay, salta el resto de Update
        }
        // si se sale del hueco pasamos al idle
        Vector2 input = new Vector2(0, Input.GetAxisRaw("Vertical")); //seteo en 0 el mov en horizontal
        _agilePlayerBehaviour.SetMovementInput(input);
        if (!_holeDetector.canDig)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);
        }
    }
}
