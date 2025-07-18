using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    //Singleton
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevents duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes. 

        if (_audioSource == null)
        {
            Debug.LogError("SFXPlayerController needs an AudioSource.");
            enabled = false;
        }
    }

    //Play SFX (no loop).
    public void PlaySFX(AudioClip clip)
    {
        if (_sfxSource != null && clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }


    // PlaySFX loop (like steps, climb).
    public void PlayLoop(AudioClip clip)
    {
        if (_audioSource != null && clip != null)
        {
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    //Stop the loop.
    public void StopLoop()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }
}

