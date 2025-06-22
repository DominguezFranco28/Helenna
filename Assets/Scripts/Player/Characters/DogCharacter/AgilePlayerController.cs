using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgilePlayerController : MonoBehaviour
{
    [SerializeField] private AgilePlayerBehaviour _agileBehaviour;
    private AgileStateMachine _agileStateMachine;

    private void Start()
    {
        _agileStateMachine = new AgileStateMachine(_agileBehaviour);
        _agileStateMachine.Initialize(_agileStateMachine.idleState);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_agileBehaviour.isInControll)
            _agileStateMachine.Update();
    }
}
