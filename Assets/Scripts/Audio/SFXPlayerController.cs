using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayerController : MonoBehaviour
{
    [Header("Configuracion de Audio")]
    [SerializeField] private AudioClip _sfxClip;
    [SerializeField] private KeyCode _playKey = KeyCode.Space; //Tecla que activa el sonido

    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        if (_audioSource == null)
        {
            Debug.LogError("SFXPlayerController requiere un AudioSource en el mismo GameObject.", this);
            enabled = false;
        }
    }
    void Update()
    {
        // Detección de la pulsación de tecla
        if (Input.GetKeyDown(_playKey))
        {
            // Validación de referencias
            if (_audioSource != null && _sfxClip != null)
            {
                _audioSource.PlayOneShot(_sfxClip); // Reproduce el clip una vez
                Debug.Log("SFX reproducido: " + _sfxClip.name);
            }
            else if (_sfxClip == null)
            {
                Debug.LogWarning("No hay AudioClip asignado al SFXPlayerController.", this);
            }
        }
    }
    public void PlaySFX() //metodo que se llama desde el script que dezplaza al jugador.
    {
            if (_audioSource !=null && _sfxClip != null) //Validacion de referencias
            {
                _audioSource.PlayOneShot(_sfxClip); //REPRODUCE EL CLIP UNA SOLA VEZ
                Debug.Log("Sfx reproducido: " + _sfxClip.name);
            }
    }
}
