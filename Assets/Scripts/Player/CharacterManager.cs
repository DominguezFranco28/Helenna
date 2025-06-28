using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //Libreria de Cinemachine
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            SFXManager.Instance.PlaySFX(_changeSFX);
            _currentIndex = (_currentIndex + 1) % characters.Length;
            ActivateCharacter(_currentIndex);
        }
        // i put a tp to make testing faster, maybe I'll leave it for the final delivery.
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
                control.SetControl(i == index);
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

        Debug.Log("All characters have been teleported to the active character.");
    }

}
