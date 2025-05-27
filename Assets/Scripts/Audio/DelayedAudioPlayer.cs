using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DelayedAudioPlayer: MonoBehaviour
{
    public AudioClip[] _clips; //Lista de sonidos para asignar en el inspector. Puede servir mucho para sonidos de ambiente
    private AudioSource _audioSource;
    private Coroutine _currentCoroutine; 
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(PlayRandom(_clips));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }
    IEnumerator PlayRandom(AudioClip[] clips)
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(1f, 10f);
            yield return new WaitForSeconds(waitTime);

            int index = UnityEngine.Random.Range(0, clips.Length);
            AudioClip clipToPlay = clips[index];

            // Clip asociado al audiosouurce del GO
            _audioSource.clip = clipToPlay;

            // Elegir punto aleatorio dentro del clip (en segundos)
            //float randomStartTime = UnityEngine.Random.Range(0f, clipToPlay.length);
            //_audioSource.time = randomStartTime;

            _audioSource.Play();
        }
    }
}

