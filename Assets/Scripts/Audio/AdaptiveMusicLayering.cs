using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicLayering : MonoBehaviour
{
    [Header("Audio Sources (Asignar en Inspector)")]
    [SerializeField] private AudioSource _baseLayerSource; // base de la musica
    [SerializeField] private AudioSource _resolutionSFXSource; // fuente de sonido para resolucion de puzzle

    [Header("Parámetros de Fade")]
    [Range(0.1f, 5.0f)]
    [SerializeField] private float _fadeDuration = 1.5f;

    private Coroutine _activeFadeCoroutine; 

    void Start()
    {
        //  Reproducir música base al iniciar (si playOnAwake está activado)
        if (_baseLayerSource != null && !_baseLayerSource.isPlaying && _baseLayerSource.playOnAwake)
        {
            _baseLayerSource.Play();
        }
    }

    //  método para reproducir un tono de resolución
    public void PlayResolutionTone()
    {
        if (_resolutionSFXSource != null && _resolutionSFXSource.clip != null)
        {
            _resolutionSFXSource.PlayOneShot(_resolutionSFXSource.clip);
        }
        else
        {
            Debug.LogWarning("resolutionSFXSource no asignado o sin clip.");
        }
    }

    // metodo apra hacer un fade de la mnusica principal
    public void FadeBaseMusicVolume(float targetVolume)
    {
        if (_baseLayerSource == null) return;

        if (_activeFadeCoroutine != null)
        {
            StopCoroutine(_activeFadeCoroutine);
        }

        _activeFadeCoroutine = StartCoroutine(FadeAudioSourceVolume(_baseLayerSource, targetVolume, _fadeDuration));
    }

    //corrutina para cambiar el volumen suavemente
    private IEnumerator FadeAudioSourceVolume(AudioSource audioSourceToFade, float finalVolume, float duration)
    {
        float currentTime = 0;
        float startingVolume = audioSourceToFade.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSourceToFade.volume = Mathf.Lerp(startingVolume, finalVolume, currentTime / duration);
            yield return null;
        }

        audioSourceToFade.volume = finalVolume;
        _activeFadeCoroutine = null;
    }
}

