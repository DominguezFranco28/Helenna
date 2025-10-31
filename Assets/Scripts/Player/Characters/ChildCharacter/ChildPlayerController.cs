using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayerController : MonoBehaviour, IHasStateMachine
{
    [SerializeField] private ChildPlayerBehaviour _childBehaviour;
    [SerializeField] private float separationDistance= 1.5f;
    [SerializeField] private float offsetY= 0.2f;

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
            if (_childBehaviour.PetDetector.CanPet && interacting) 
            {
                //Alinear personajes antes de la animacion de pet
               InputManager.Instance.LockInputs(); //lockeo inputs para que no se muevan durante la animacion
                var nina = CharacterManager.Instance.GetCharacterTransform("ChildPlayer");
                var rex = CharacterManager.Instance.GetCharacterTransform("DogPlayer");
                if (nina != null && rex != null)
                {
                    //DIRECCION VIEJO PARA PONERLA EN LOS COSTADOS DE REX, VOLVER A IMPLEMENTAR CUANDO ESTEN SPRITES DE AMBOS CORREGIDOS
                    //SINO EL FLIP QUEDA RARO (Reemplazar valor hardcodeado -1 por direction)

                    //float direction = nina.transform.position.x < rex.transform.position.x ? -1f : 1f; //ajusto la direccion de la anim segun la pos en X de los personajes
                    float directionY = nina.transform.position.y < rex.transform.position.y ? 1f : -1f;
                    Vector2 offset = new Vector2(-1 * separationDistance, offsetY); //0.4 para ajustar la altura en Y del pet, puede mejorarse
                    StartCoroutine(CharacterManager.Instance.AlignCharacters(nina, rex, offset, 0.10f)); //ojo ajustar el pivote para que quede bien alineado
                    _childBehaviour.Animator.SetFloat("Horizontal", -1f); 
                    _childBehaviour.Animator.SetFloat("Vertical", directionY); 
                    _childBehaviour.Animator.SetFloat("Speed", 0.5f); //seteo la direccion de la animacion
                    StartCoroutine(ResetAnimator(2f)); //un delay antes de reestablecer los valores del animator
                    Debug.Log("Aligning characters");
                }
                else
                {
                    Debug.LogWarning("Nina o Rex no encontrados al intentar alinear.");
                }
                return;

                /* //HERRAMIENTA DEBUGEO, TP A TODOS LOS PERSONAJES A LA POSICION DEL ACTIVO
                 if (Input.GetKey(KeyCode.LeftShift))
                 {
                     CharacterManager.Instance.TeleportAllToCurrent();
                 }*/
            }
                if (_childBehaviour.LeverDetector.CanActivate && interacting)
                {
                    StateMachine.TransitionTo(StateMachine.actionState);
                    return;
                }
        }
    }
    private IEnumerator ResetAnimator(float delay)
    {
        yield return new WaitForSeconds(delay);
        _childBehaviour.Animator.SetFloat("Horizontal", 0f); //reseteo la direccion de la animacion
        _childBehaviour.Animator.SetFloat("Vertical", -1f); //reseteo la direccion de la animacion
        _childBehaviour.Animator.SetFloat("Speed", 0f); //reseteo la direccion de la animacion
    }
}
