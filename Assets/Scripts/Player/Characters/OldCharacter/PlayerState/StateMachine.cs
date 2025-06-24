 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
    // en el libro aclara que el StateMachine no necesariamente hereda de monobehavoir.
    // Esto es asi porque es un patron de ddiseno no solo pensado para unity,
    // ademas de qwue responde a una buena practica en cuando a Separacion de responsabilidades,
    // mejora la org y mantiene el patrón limpio y reutilizable en diversos casos.


    //Como no hereda de MonoBehavoir, es necesario usar constructores para setear cada instancia de los estados deseados.
    //No puede usarse el getcomponent porque este es un metodo heredador de monobehaviour
{
    public MoveState moveState;
    public IdleState idleState;
    public ImpulseState impulserState;

    public IState CurrentState { get; private set; } //Solo lectura. Objeto externo puede setear el metodo Initialize para establecer un state default
    public StateMachine(PlayerBehaviour player)
    {
        this.moveState = new MoveState(player,this); 
        this.idleState = new IdleState(player,this);
        this.impulserState = new ImpulseState(player,this);


        //Fue necesario agregar el this. Paso esta instanciacion de la clase StateMachine como parametro para que todos los estados conozca la UNICA StateMachina de estados existentes, y no me construyan nuevas en cada instanciacion.
    }


    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter(); //Metodos Enter . Update y exit de la interfaz de Istate, para gestionar la entrad ay salida de los estados.

    }
    public void TransitionTo (IState nextState)
    {
        CurrentState.Exit(); 
        CurrentState= nextState;
        nextState.Enter();

    }
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
