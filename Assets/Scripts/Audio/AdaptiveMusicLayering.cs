using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicLayering : MonoBehaviour
{
    [Header("Audio Sources (Asignar en Inspector)")]
    [SerializeField] private AudioSource baseLayerSource; // 🔄 Antes había dos, ahora solo la base
    [SerializeField] private AudioSource resolutionSFXSource; // ✅ NUEVO: fuente de sonido para el tono de resolución

    [Header("Parámetros de Fade")]
    [Range(0.1f, 5.0f)]
    [SerializeField] private float fadeDuration = 1.5f;

    private Coroutine activeFadeCoroutine; // 🔄 Se mantiene por si querés hacer fade en la base

    void Start()
    {
        //  Reproducir música base al iniciar (si playOnAwake está activado)
        if (baseLayerSource != null && !baseLayerSource.isPlaying && baseLayerSource.playOnAwake)
        {
            baseLayerSource.Play();
        }
    }

    //  método para reproducir un tono de resolución
    public void PlayResolutionTone()
    {
        if (resolutionSFXSource != null && resolutionSFXSource.clip != null)
        {
            resolutionSFXSource.PlayOneShot(resolutionSFXSource.clip);
        }
        else
        {
            Debug.LogWarning("resolutionSFXSource no asignado o sin clip.");
        }
    }

    // metodo apra hace run fade de la mnusica principal
    public void FadeBaseMusicVolume(float targetVolume)
    {
        if (baseLayerSource == null) return;

        if (activeFadeCoroutine != null)
        {
            StopCoroutine(activeFadeCoroutine);
        }

        activeFadeCoroutine = StartCoroutine(FadeAudioSourceVolume(baseLayerSource, targetVolume, fadeDuration));
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
        activeFadeCoroutine = null;
    }
}

