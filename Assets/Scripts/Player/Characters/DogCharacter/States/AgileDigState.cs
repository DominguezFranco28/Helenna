using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileDigState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private AgileTriggerDetector _holeDetector;

    private float _digDelay = 1f; 
    private float _digTimer;
    private bool _delayCompleted;
    private GameObject _pickedObject;

    public AgileDigState(AgilePlayerBehaviour agilePlayerBehaviour, AgileStateMachine agileStateMachine)
    {
        this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;
        this._holeDetector = agilePlayerBehaviour.TriggerDetector;

    }

    public void Enter()
    {
        Debug.Log("You entered the state: DIG");
        
        _agilePlayerBehaviour.Animator.SetBool("Dig", true); 
        _agilePlayerBehaviour.StopMovement();
        _agilePlayerBehaviour.SetMovementEnabled(false);
        _digTimer = 0f;
        _delayCompleted = false;
        SFXManager.Instance.PlaySFX(_agilePlayerBehaviour.DigSFXClip);
    }

    public void Exit()
    {
        Debug.Log("You left the state: DIG");

            _agilePlayerBehaviour.Animator.SetBool("Dig", false);
        
    }
    public void Object(GameObject gameObject)
    {
        _pickedObject = gameObject;
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

        /*Vector2 input = new Vector2(0, Input.GetAxisRaw("Vertical")); //set the horizontal move to 0
        _agilePlayerBehaviour.SetMovementInput(input);*/

        // If we leave the gap we go to idle
        if (_agilePlayerBehaviour.CurrentHole != null)
        {
            _agilePlayerBehaviour.transform.position = _agilePlayerBehaviour.CurrentHole.exitHole.transform.position;
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);

            _agilePlayerBehaviour.CurrentHole.Use();

            //logica vieja de agarrado de objetos 
            //if (_pickedObject != null) //si desde item state recibe el objeto como parametro del metodo Object, devuelve a ese estado para poder agarrar y soltar objetos luego del salto
            //{
            //     //vacio el parametro despues de la transicion. Vuelve a renovarse desde el itemstate si corresponmde
            //    _agileStateMachine.TransitionTo(_agileStateMachine.itemState);
            //}

        }
    }
}
