using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileDigState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private AgileTriggerDetector _holeDetector;
    private Vector2 _direction; //direccion del dig, la misma que la del salto
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
        _direction = _agilePlayerBehaviour.LastMovementInput.normalized;
        _agilePlayerBehaviour.Animator.SetTrigger("Dig");
        _agilePlayerBehaviour.Animator.SetBool("IsDigging", true);
        _agilePlayerBehaviour.Animator.SetFloat("ThrowHorizontal", _direction.x);
        _agilePlayerBehaviour.Animator.SetFloat("ThrowVertical", _direction.y);
        _agilePlayerBehaviour.StopMovement();
        _agilePlayerBehaviour.SetMovementEnabled(false);
        _digTimer = 0f;
        _delayCompleted = false;
        SFXManager.Instance.PlaySFX(_agilePlayerBehaviour.DigSFXClip);

    }

    public void Exit()
    {
        Debug.Log("You left the state: DIG");


    }
    public void Object(GameObject gameObject)
    {
        _pickedObject = gameObject;
    }
    public void Update()
    {
        // Wait for delay
        // Actualizo blend todo el tiempo
        _agilePlayerBehaviour.Animator.SetFloat("ThrowHorizontal", _direction.x);
        _agilePlayerBehaviour.Animator.SetFloat("ThrowVertical", _direction.y);

        // Avanza el timer del cavado
        _digTimer += Time.deltaTime;

        if (_digTimer < _digDelay)
        {
            //delay hasta que termine a animacion
            return;
        }
        _agilePlayerBehaviour.Animator.SetBool("IsDigging", false);

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
