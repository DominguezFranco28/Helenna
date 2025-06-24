using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //Libreria de Cinemachine
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField] private GameObject[] characters;
    private int _currentIndex = 0;
    //Prop publica con getter para tener accedo al personaje actual y controlador, sin acoplar todo en el ciclo for.
    //public GameObject CurrentCharacter => characters[_currentIndex];

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private AudioClip _changeSFX;

    void Awake()

    {
        Debug.Log("CharacterManager: Awake() ejecutado");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados, mantenemos logica singleton
            return;
        }

        Instance = this;
        ActivateCharacter(_currentIndex);
        //DontDestroyOnLoad(gameObject); // Para que persista entre scenas.
    }

    void Update()
    {
        //Debug.Log("Estado actual del juego: " + GameStateManager.Instance.CurrentState);
        //if (GameStateManager.Instance.IsGamePaused()) return; //Hago tambienq ue este controlador respeta la pausa, sino te deja cambiar de pj mientras lees
        //Debug.Log("Estado actual del juego: " + GameStateManager.Instance.CurrentState);
        //Evito que se pueda cambiar de pj mientras se esta en pausa
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            SFXManager.Instance.PlaySFX(_changeSFX);
            _currentIndex = (_currentIndex + 1) % characters.Length;
            ActivateCharacter(_currentIndex);
        }
        // Puse un tp para hacer el testeo mas rapido, tal vez lo deje para la entrega final.
        if (Input.GetKeyDown(KeyCode.LeftShift) && (characters[_currentIndex].name == "OldPlayer"))
        {
            TeleportAllToCurrent();
        }
    }

    void ActivateCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            IControllable control = characters[i].GetComponent<IControllable>();
            if (control != null)
            {
                control.SetControl(i == index); //Esto es igual a true, solo para el personaje que esta en el index en este ciclo del for,
                                                //todos los demas quedan en false asi que no se pueden mover por su Behaviour
            }
            if (_virtualCamera != null)
            {
                _virtualCamera.Follow = characters[index].transform; //Para que la VC siga al personaje que este en control dentro del ciclo
            }
        }

    }
    void TeleportAllToCurrent()
    {
        Transform targetPosition = characters[_currentIndex].transform;

        for (int i = 0; i < characters.Length; i++)
        {
            if (i != _currentIndex)
            {
                characters[i].transform.position = targetPosition.position;
            }
        }

        Debug.Log("Todos los personajes han sido teletransportados al personaje activo.");
    }

    //public IControllable GetCurrentControllable()
    //{
    //    return CurrentCharacter.GetComponent<IControllable>();
    //}
}
