using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    //Singleton del SFX MANAGER
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados, mantenemos logica singleton
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Para que persista entre scenas.

        if (_audioSource == null)
        {
            Debug.LogError("SFXPlayerController requiere un AudioSource asignado.");
            enabled = false;
        }
    }

    //Metodo para reproducir un SFX simple (no loop).

    public void PlaySFX(AudioClip clip)
    {
        if (_sfxSource != null && clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }


    // Reproduce un SFX en loop (como pasos, escalada).
    public void PlayLoop(AudioClip clip)
    {
        if (_audioSource != null && clip != null)
        {
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    //Detiene el loop.
    public void StopLoop()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }
}

