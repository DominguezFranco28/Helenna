using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //Cinemachine Library

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField] private GameObject[] characters;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private AudioClip _changeSFX;
    private int _currentIndex = 0;

    void Awake()
    {
        Debug.Log("CharacterManager: Awake() ejecutado");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevents duplicate
            return;
        }

        Instance = this;
        ActivateCharacter(_currentIndex);
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.ChangeCharacterPressed += OnChangeCharacter;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.ChangeCharacterPressed -= OnChangeCharacter;
    }

    private void OnChangeCharacter()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;

        SFXManager.Instance.PlaySFX(_changeSFX);
        _currentIndex = (_currentIndex + 1) % characters.Length;
        ActivateCharacter(_currentIndex);
    }


    void ActivateCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            IControllable control = characters[i].GetComponent<IControllable>();
            if (control != null)
            {
                Debug.Log("ActivateCharacter: " + characters[i].name);
                control.SetControl(i == index);
                control.SetMovementEnabled(i == index);
                //This is equal to true, only for the character that is at the index in this for loop,
                //all the others are set to false so they cannot move due to their Behavior
            }
            if (_virtualCamera != null)
            {
                _virtualCamera.Follow = characters[index].transform;
                //For the VC to follow the character that is in control within the cycle
            }
        }

    }
   public void TeleportAllToCurrent()
    {
        Transform targetPosition = characters[_currentIndex].transform;
        int changePosition = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            if (i != _currentIndex && changePosition < 1)
            {
              
                characters[i].transform.position = targetPosition.position + Vector3.right;
                changePosition++;
            }
            else if (i != _currentIndex)
            {
                characters[i].transform.position = targetPosition.position + Vector3.left;
            }
        }

        Debug.Log("All characters have been teleported to the active character.");
    }

    public void JoinToTeam (GameObject newPlayer)
    {
        // Convertimos el array a una lista para poder agregar elementos
        List<GameObject> characterList = new List<GameObject>(characters);

        // Agregamos el nuevo personaje
        characterList.Add(newPlayer);

        // Volvemos a convertirlo en array
        characters = characterList.ToArray();

        Debug.Log($"{newPlayer.name} se ha unido al equipo. Total de personajes: {characters.Length}");
    }
}
