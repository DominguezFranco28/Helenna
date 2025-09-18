using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileDigState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private AgileTriggerDetector _holeDetector;

    private float _digDelay = 0.3f; 
    private float _digTimer;
    private bool _delayCompleted;
    private GameObject _pickedObject;

    private bool subbed = false;

    public AgileDigState(AgilePlayerBehaviour agilePlayerBehaviour, AgileStateMachine agileStateMachine)
    {
        this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;
        this._holeDetector = agilePlayerBehaviour.HoleDetector;

    }
    private void OnMove(Vector2 movement)
    {
        _agilePlayerBehaviour.SetMovementInput(new Vector2(0, movement.y));
    }

    public void Enter()
    {
        Debug.Log("You entered the state: DIG");
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = false;
                InputManager.Instance.Move += OnMove;
            }
        }
        
        _agilePlayerBehaviour.Animator.SetBool("Dig", true); 
        _digTimer = 0f;
        _delayCompleted = false;
        SFXManager.Instance.PlaySFX(_agilePlayerBehaviour.DigSFXClip);
    }

    public void Exit()
    {
        Debug.Log("You left the state: DIG");

        if (_agilePlayerBehaviour.CanDig == false)
        {
            _agilePlayerBehaviour.Animator.SetBool("Dig", false);
        }
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
        if (!_agilePlayerBehaviour.CanDig)
        {
            if (_pickedObject != null) //si desde item state recibe el objeto como parametro del metodo Object, devuelve a ese estado para poder agarrar y soltar objetos luego del salto
            {
                 //vacio el parametro despues de la transicion. Vuelve a renovarse desde el itemstate si corresponmde
                _agileStateMachine.TransitionTo(_agileStateMachine.itemState);
            }
            else
                _agileStateMachine.TransitionTo(_agileStateMachine.moveState);
           
        }
    }
}
