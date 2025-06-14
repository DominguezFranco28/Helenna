using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusic_Layering : MonoBehaviour
{
    [Header("Audio Sources (Asignar en Inspector)")]
    [SerializeField] private AudioSource baseLayerSource;    // Arrastrar aquí el AudioSource de la capa base
    [SerializeField] private AudioSource tensionLayerSource; // Arrastrar aquí el AudioSource de la capa de tensión




    [Header("Parámetros de Fade")]
    [Range(0.1f, 5.0f)] // Slider en el Inspector para ajustar el tiempo de fade
    [SerializeField] private float fadeDuration = 1.5f;

    private Coroutine activeFadeCoroutine; // Referencia a la corrutina de fade activa




    void Start()
    {
        // Asegurar la configuración inicial correcta al iniciar la escena
        if (baseLayerSource != null && !baseLayerSource.isPlaying && baseLayerSource.playOnAwake)
        {
            // Si PlayOnAwake está marcado pero por alguna razón no sonó (ej. objeto desactivado y luego activado)
            baseLayerSource.Play();
        }




        if (tensionLayerSource != null && tensionLayerSource.clip != null)
        {
            tensionLayerSource.volume = 0f; // La capa de tensión inicia completamente en silencio
            if (!tensionLayerSource.isPlaying)
            {
                tensionLayerSource.Play(); // Iniciar la reproducción en silencio para que esté sincronizada
                                           // y lista para que su volumen suba cuando se necesite.
            }
        }
        else if (tensionLayerSource == null)
        {
            Debug.LogError("ERROR: TensionLayerSource no ha sido asignado en el script AdaptiveMusic_Layering!");
        }
    }




    // Método público para ser llamado desde otros scripts y activar/desactivar la capa de tensión
    public void SetTensionLayerActive(bool activateTension)
    {
        if (tensionLayerSource == null)
        {
            Debug.LogWarning("Advertencia: Se intentó activar/desactivar una capa de tensión no asignada.");
            return;
        }




        float targetVolume = activateTension ? 1.0f : 0.0f; // Determinar el volumen objetivo




        // Si ya existe una corrutina de fade ejecutándose, la detenemos para evitar conflictos
        if (activeFadeCoroutine != null)
        {
            StopCoroutine(activeFadeCoroutine);
        }

        // Iniciamos la nueva corrutina que realizará el fade de volumen
        activeFadeCoroutine = StartCoroutine(FadeAudioSourceVolume(tensionLayerSource, targetVolume, fadeDuration));
    }




    // Corrutina que maneja el cambio gradual de volumen
    private IEnumerator FadeAudioSourceVolume(AudioSource audioSourceToFade, float finalVolume, float duration)
    {
        float currentTime = 0;
        float startingVolume = audioSourceToFade.volume;




        // Bucle que se ejecuta hasta que se completa la duración del fade
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime; // Incrementar el tiempo basado en el tiempo real transcurrido
            // Interpolar linealmente el volumen desde el inicial al final, basado en el progreso del tiempo
            audioSourceToFade.volume = Mathf.Lerp(startingVolume, finalVolume, currentTime / duration);
            yield return null; // Esperar al siguiente frame antes de continuar el bucle
        }

        audioSourceToFade.volume = finalVolume; // Asegurar que el volumen llegue exactamente al valor objetivo
        activeFadeCoroutine = null; // Limpiar la referencia a la corrutina, ya que ha terminado
    }
}
