using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicLayering : MonoBehaviour
{
    
    [SerializeField] private AudioSource _baseLayerSource; 
    [SerializeField] private AudioSource _resolutionSFXSource; 

    [Header("Fade parameters")]
    [Range(0.1f, 5.0f)]
    [SerializeField] private float _fadeDuration = 1.5f;
    private Coroutine _activeFadeCoroutine; 


    void Start()
    {
        // Play background music on startup (if playOnAwake is enabled)
        if (_baseLayerSource != null && !_baseLayerSource.isPlaying && _baseLayerSource.playOnAwake)
        {
            _baseLayerSource.Play();
        }
    }

    //  Method for reproducing a resolution tone
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

    // method to fade out the main music
    public void FadeBaseMusicVolume(float targetVolume)
    {
        if (_baseLayerSource == null) return;

        if (_activeFadeCoroutine != null)
        {
            StopCoroutine(_activeFadeCoroutine);
        }

        _activeFadeCoroutine = StartCoroutine(FadeAudioSourceVolume(_baseLayerSource, targetVolume, _fadeDuration));
    }

    //coroutine to change the volume smoothly
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

