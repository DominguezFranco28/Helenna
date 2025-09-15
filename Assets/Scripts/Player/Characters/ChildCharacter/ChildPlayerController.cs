using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayerController : MonoBehaviour
{
    [SerializeField] private ChildPlayerBehaviour _childBehaviour;
    [SerializeField] private ChildTriggerDetector _childTriggerDetector;
    private ChildStateMachine _childStateMachine;
    
    private bool interacting = false;
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
        _childStateMachine = new ChildStateMachine(_childBehaviour, _childTriggerDetector);
        _childStateMachine.Initialize(_childStateMachine.idleState);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_childBehaviour.IsInControll)
        {
            _childStateMachine.Update();
            // Detect enter to climb
            if (_childTriggerDetector.CanClimb)
            {

                _childStateMachine.TransitionTo(_childStateMachine.climbState);
                return;
            }

            if (_childTriggerDetector.CanActivate && interacting)
            {
                _childStateMachine.TransitionTo(_childStateMachine.actionState);
                return;
            }
        }
    }
}
