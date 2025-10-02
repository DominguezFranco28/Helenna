using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayerController : MonoBehaviour , IHasStateMachine
{
    [SerializeField] private ChildPlayerBehaviour _childBehaviour;
    [SerializeField] private ChildTriggerDetector _childTriggerDetector;
    
    private bool interacting = false;
    public ChildStateMachine StateMachine { get; private set; }
    public IState CurrentState => StateMachine.CurrentState;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.InteractPressed += InteractPressed;
        }

    }
    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.InteractPressed -= InteractPressed;
        }

    }
    private void InteractPressed()
    {
        if (!_childBehaviour.IsInControll) return;
        interacting = true;
        if (InputManager.Instance != null)
            InputManager.Instance.InvokeAction(() => interacting = false, 0.1f);
    }

    private void Start()
    {
        StateMachine = new ChildStateMachine(_childBehaviour, _childTriggerDetector);
        StateMachine.Initialize(StateMachine.idleState);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_childBehaviour.IsInControll)
        {
            StateMachine.Update();
            // Detect enter to climb
            if (_childTriggerDetector.CanClimb)
            {

                StateMachine.TransitionTo(StateMachine.climbState);
                return;
            }

            if (_childTriggerDetector.CanActivate && interacting)
            {
                StateMachine.TransitionTo(StateMachine.actionState);
                return;
            }
           /* //HERRAMIENTA DEBUGEO, TP A TODOS LOS PERSONAJES A LA POSICION DEL ACTIVO
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CharacterManager.Instance.TeleportAllToCurrent();
            }*/
        }
    }
}
