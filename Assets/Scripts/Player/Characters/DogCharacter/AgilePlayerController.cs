using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgilePlayerController : MonoBehaviour
{
    [SerializeField] private AgilePlayerBehaviour _agileBehaviour;
    [SerializeField ]private PlatformDetector _platformDetector;
    private AgileStateMachine _agileStateMachine;
    private void Start()
    {
        _agileStateMachine = new AgileStateMachine(_agileBehaviour, _platformDetector);
        _agileStateMachine.Initialize(_agileStateMachine.idleState);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_agileBehaviour.isInControll)
            _agileStateMachine.Update();
    }
}
