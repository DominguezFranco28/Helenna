using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPipe : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private AnchorDetector _anchorDetector;
    private OldStateMachine _oldStateMachine;



    public HookPipe(OldPlayerBehaviour player, OldStateMachine oldStateMachine, AnchorDetector anchorDetector)
    {
        _oldPlayerBehaviour = player;
        _anchorDetector = anchorDetector;
        _oldStateMachine = oldStateMachine;
    }


    public void Enter()
    {
        Debug.Log("You entered the state: HOOKPIPE");
    }

    public void Exit()
    {
        Debug.Log("You left the state: HOOKPIPE");
    }

    public void Update()
    {
        
    }
}
