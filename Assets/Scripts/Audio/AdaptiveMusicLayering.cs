using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicLayering : MonoBehaviour
{

    [Header("Audiosources para crossfade")]
    [SerializeField] private AudioSource _layerOneSource; 
    [SerializeField] private AudioSource _layerTwoSource;
    [Header("Musica base con layering")]
    [SerializeField] private AudioSource _resolutionSFXSource;
    public static AdaptiveMusicLayering Instance { get; private set; }
    [Header("Fade parameters")]
    [Range(0.1f, 5.0f)]
    [SerializeField] private float _fadeDuration = 1.5f;
    private Coroutine _activeFadeCoroutine;
    private AudioSource _currentMusicLayer;
    private const float TARGET_VOLUME = 1.0f; //vol objetivo de la musica cuando ya arranco
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevents duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes. 

        if (_layerOneSource == null)
        {
           // Debug.LogError("Se encesita un tema principal");
            enabled = false;
        }
        if (_layerTwoSource == null)
        {
            Debug.LogWarning("_layerTwoSource no asignado. La transición de música no funcionará.");
        }
    }
    void Start()
    {
        // **MODIFICACIÓN:** Asegurarse de que ambos clips se inicialicen y el segundo esté silenciado.

        if (_layerOneSource != null)
        {
            // Inicializamos el volumen base
            _layerOneSource.volume = 0f;
            if (!_layerOneSource.isPlaying)
            {
                _layerOneSource.Play(); // Ambos empiezan a sonar (loop)
            }
            // La capa uno es la inicial
            _currentMusicLayer = _layerOneSource;
            FadeCurrentMusicVolume(TARGET_VOLUME); //levamos el sonido a 1
        }

        if (_layerTwoSource != null)
        {
            // La capa dos empieza silenciada
            _layerTwoSource.volume = 0f;
            if (!_layerTwoSource.isPlaying)
            {
                _layerTwoSource.Play(); // Ambos empiezan a sonar (loop)
            }
        }
    }

    //  llamar antes de TransitionManager.LoadScene()
    public void FadeOutMusicBeforeSceneChange()
    {
        // Detiene cualquier fade actual 
        if (_activeFadeCoroutine != null)
        {
            StopCoroutine(_activeFadeCoroutine);
        }

        //  fade-out de la musica actual
        _activeFadeCoroutine = StartCoroutine(FadeOutAndStop(_currentMusicLayer, _fadeDuration));
    }

    // corrutina para el fade-out final
    private IEnumerator FadeOutAndStop(AudioSource audioSourceToFade, float duration)
    {
        float currentTime = 0;
        float startingVolume = audioSourceToFade.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            // Lerp de volumen actual a 0
            audioSourceToFade.volume = Mathf.Lerp(startingVolume, 0f, currentTime / duration);
            yield return null;
        }

        audioSourceToFade.volume = 0f;
        // detenemos la música una vez que termina el fade-out
      //  audioSourceToFade.Stop();
        _activeFadeCoroutine = null;
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
    public void FadeCurrentMusicVolume(float targetVolume)
    {
        if (_currentMusicLayer == null) return;

        if (_activeFadeCoroutine != null)
        {
            StopCoroutine(_activeFadeCoroutine);
        }

        _activeFadeCoroutine = StartCoroutine(FadeAudioSourceVolume(_currentMusicLayer, targetVolume, _fadeDuration));
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
    public void TransitionToLayerOne(bool toLayerOne)
    {
        // Si no tenemos las dos capas, salimos.
        if (_layerOneSource == null || _layerTwoSource == null)
        {
            Debug.LogWarning("Faltan capas de música para realizar la transición.");
            return;
        }

        // Determinar la capa que debe sonar y la que debe silenciarse
        AudioSource fadeInSource = toLayerOne ? _layerOneSource : _layerTwoSource;
        AudioSource fadeOutSource = toLayerOne ? _layerTwoSource : _layerOneSource;

        // Si ya estamos en el estado deseado, salimos.
        if (fadeInSource == _currentMusicLayer)       
            return;
        

        // Si hay una transición en curso, la detenemos para iniciar la nueva.
        if (_activeFadeCoroutine != null)
        {
            StopCoroutine(_activeFadeCoroutine);
        }

        // Iniciamos el crossfade
        _activeFadeCoroutine = StartCoroutine(CrossfadeLayers(fadeInSource, fadeOutSource, _fadeDuration));
    }
    private IEnumerator CrossfadeLayers(AudioSource fadeInSource, AudioSource fadeOutSource, float duration)
    {
        _currentMusicLayer = fadeInSource; // Actualizamos la capa actual inmediatamente
        float currentTime = 0;

        // la capa que entra debe llegar a volumen 1 (o el volumen deseado si lo haces más complejo)
        // y la que sale debe llegar a 0.
        float startVolumeIn = fadeInSource.volume; // Debería ser 0
        float startVolumeOut = fadeOutSource.volume; // Debería ser 1 (si estaba sonando)

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            // La capa que entra aumenta su volumen (0 -> 1)
            fadeInSource.volume = Mathf.Lerp(startVolumeIn, 1f, t);
            // La capa que sale disminuye su volumen (1 -> 0)
            fadeOutSource.volume = Mathf.Lerp(startVolumeOut, 0f, t);

            yield return null;
        }

        // volumenes finales
        fadeInSource.volume = 1f;
        fadeOutSource.volume = 0f;
        _activeFadeCoroutine = null;
    }

    public float GetFadeDuration()
    {
        return _fadeDuration;
    }
}

