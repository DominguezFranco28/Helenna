using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerController : MonoBehaviour
{
    [SerializeField]private OldPlayerBehaviour _playerBehaviour;
    private OldStateMachine _myStateMachine;


    private void Start()
    {
        _myStateMachine = new OldStateMachine(_playerBehaviour); //Le paso los el parametro del player vehavbior
                                                                                     //Lo pason en el constructor por los State no heredan de monobehavoiur

        // Este ejemplo solo inicia en uno. Los estados pueden referenciar la StateMachine si querés hacer transiciones desde adentro.
        _myStateMachine.Initialize(_myStateMachine.idleState);
        //Recordar, el StateMachine ya tiene los estados creados en su constructor, no hace falta volver a instanciarlos aca.
    }
    private void Update()
    {
        /*if (GameStateManager.Instance.IsGamePaused()) return;*/ //Respeto el State de pausa, si esta pausado que ni entre en la maquina de estado
        //Me da problema con el trnaisison de escenas, revisar esto
        if (_playerBehaviour.isInControll) //Tuve que agregarle la booleana aca tambien, porque sino me frenaba el movimiento del viejo pero me dejaba hacer el impulso apretando la q
        {
        
            _myStateMachine?.Update();

        }
    }
}
