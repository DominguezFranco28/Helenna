using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private PlayerBehaviour _playerBehaviour;
    private StateMachine _myStateMachine;
    private ArmImpulser _telekinesisForce;

    private void Start()
    {
        _telekinesisForce = GetComponent<ArmImpulser>(); //Referencia para ejecutar la accion de inpacto del brazo.
        _myStateMachine = new StateMachine(_playerBehaviour); //Le paso los 3 parametros ahora para tener en cuenta el brazo del viejo.
                                                                                     //Lo pason en el constructor por los State no heredan de monobehavoiur

        // Este ejemplo solo inicia en uno. Los estados pueden referenciar la StateMachine si querés hacer transiciones desde adentro.
        _myStateMachine.Initialize(_myStateMachine.idleState);
        //Recordar, el StateMachine ya tiene los estados creados en su constructor, no hace falta volver a instanciarlos aca.
    }
    private void Update()
    {
        _myStateMachine?.Update();
    }
}
