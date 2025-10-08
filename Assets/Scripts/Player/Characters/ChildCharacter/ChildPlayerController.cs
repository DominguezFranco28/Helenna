using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayerController : MonoBehaviour , IHasStateMachine
{
    [SerializeField] private ChildPlayerBehaviour _childBehaviour;
    
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
        _childBehaviour.InitializeDetectors(); //asignar los detectores antes de crear la statemachine y evitar  ref nulas    
      StateMachine = new ChildStateMachine(_childBehaviour);
        StateMachine.Initialize(StateMachine.idleState);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_childBehaviour.IsInControll)
        {
            StateMachine.Update();
            // Detect enter to climb
            if (_childBehaviour.ClimbDetector.CanClimb)
            {

                StateMachine.TransitionTo(StateMachine.climbState);
                return;
            }

            if (_childBehaviour.LeverDetector.CanActivate && interacting)
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
