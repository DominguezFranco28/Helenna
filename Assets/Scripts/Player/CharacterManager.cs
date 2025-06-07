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
    
}
