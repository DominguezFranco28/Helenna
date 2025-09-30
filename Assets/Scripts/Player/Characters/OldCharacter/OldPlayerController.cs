using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerController : MonoBehaviour
{
    [SerializeField] private OldPlayerBehaviour _playerBehaviour; //referencio la instancia de las clase, el jugador
    [SerializeField] private TriggerDetector _triggerDetector;
    [SerializeField] private GrabObject _grabObject;
    [SerializeField] private AnchorDetector _anchorDetector;
    [SerializeField] private AgilePlayerController _rexController;
    private OldStateMachine _myStateMachine;
    //private bool interacting = false;
    //private void OnEnable()
    //{
    //    if (InputManager.Instance != null)
    //    {
    //        InputManager.Instance.InteractPressed += InteractPressed;
    //    }

    //}
    //private void OnDisable()
    //{
    //    if (InputManager.Instance != null)
    //    {
    //        InputManager.Instance.InteractPressed -= InteractPressed;
    //    }

    //}
    //private void InteractPressed()
    //{
    //    if (!_playerBehaviour.IsInControll) return;
    //    interacting = true;
    //    if (InputManager.Instance != null)
    //        InputManager.Instance.InvokeAction(() => interacting = false, 0.1f);
    //}

    private void Start()
    {
        _myStateMachine = new OldStateMachine(_playerBehaviour, _triggerDetector, _grabObject, _anchorDetector, _rexController); 
        _myStateMachine.Initialize(_myStateMachine.idleState);
        //Remember, the StateMachine already has the states created in the constructor, no need to instantiate it again here
    }
    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_playerBehaviour.IsInControll) //Tuve que agregarle la booleana aca tambien, porque sino me frenaba el movimiento del viejo pero me dejaba hacer el impulso apretando la q
        {
        
            _myStateMachine?.Update();
            //if (_grabObject.PickedObject == null && _grabObject.InColision)
            //{
            //    _myStateMachine.TransitionTo(_myStateMachine.holdItemState);
            //}
            //if (_triggerDetector.CanActivate && interacting)
            //{
            //    _myStateMachine.TransitionTo(_myStateMachine.actionState);
            //    return;
            //}
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CharacterManager.Instance.TeleportAllToCurrent();
            }
            //TEST FUNCIONANMIENTO SWITCH MANO
        }

    }
}
