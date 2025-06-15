using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //Libreria de Cinemachine
public class CharacterManager : MonoBehaviour
{
    public GameObject[] characters;
    private int _currentIndex = 0;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    void Start()
    {
        ActivateCharacter(_currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _currentIndex = (_currentIndex + 1) % characters.Length;
            ActivateCharacter(_currentIndex);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && (characters[_currentIndex].name == "OldPlayer")) // puedo limitar el tp solo al viejo, tengo que ver si lo dejo esto
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
                control.SetControl(i == index); //Esto es igual a true, solo para el personaje que esta en el index en este ciclo del for, todos los demas quedan en false
            }
            if (_virtualCamera != null)
            {
                _virtualCamera.Follow = characters[index].transform;
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

}
