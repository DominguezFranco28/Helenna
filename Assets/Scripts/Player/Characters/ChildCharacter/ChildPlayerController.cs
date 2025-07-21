using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayerController : MonoBehaviour
{
    [SerializeField] private ChildPlayerBehaviour _childBehaviour;
    [SerializeField] private ChildTriggerDetector _childTriggerDetector;
    private ChildStateMachine _childStateMachine;

    private void Start()
    {
        _childStateMachine = new ChildStateMachine(_childBehaviour, _childTriggerDetector);
        _childStateMachine.Initialize(_childStateMachine.idleState);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_childBehaviour.isInControll)
        {
            _childStateMachine.Update();
            // Detect enter to climb
            if (_childTriggerDetector.CanClimb && Input.GetKeyDown(KeyCode.E))
            {
                _childStateMachine.TransitionTo(_childStateMachine.climbState);
                return;
            }
            else if (_childTriggerDetector.CanActivate && Input.GetKeyDown(KeyCode.E))
            {

                _childStateMachine.TransitionTo(_childStateMachine.actionState);
                return;
            }
        }
    }
}
