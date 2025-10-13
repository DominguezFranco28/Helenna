using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CameraZoom : MonoBehaviour
{
    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private TimeLineTrigger _timeLineTrigger;
    public float targetZoom = 5f;
    public float zoomSpeed = 2f;
    private float originalZoom;
    private bool zooming = false;
    private bool resetting = false;

    [Header("Shake Settings")]
    public float shakeDuration = 0.5f;
    public float shakeAmplitude = 2f;
    public float shakeFrequency = 2f;

    [Header("Post Process")]
    public Volume postProcessVolume;
    public AudioClip haroldBreakSound;
    private ChromaticAberration chromaticAberration;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    [Header("Cinematic Controller")]
    public CinematicController cinematicController;

    // Flag para evitar triggers múltiples
    private bool haroldBreak = false;

    void Start()
    {
        // guardo el zoom original de la vc
        originalZoom = virtualCamera.m_Lens.OrthographicSize;

        // componente de ruido (screenshake)
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // efecto de aberración cromática del volume (tener objeto en escena)
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGet(out chromaticAberration);
        }
    }

    public void StartZoom()
    {
        zooming = true;
        resetting = false;
    }

    public void TriggerHaroldBreak()
    {
        if (!haroldBreak)
            StartCoroutine(HaroldBreakRoutine());
    }

    private IEnumerator HaroldBreakRoutine()
    {
        haroldBreak = true;

        // Variables para el loop
        float shakeTimer = 0f;
        float chromaIntensity = 0f;
        // Activar shake
        if (perlinNoise != null)
        {
            perlinNoise.m_AmplitudeGain = shakeAmplitude;
            perlinNoise.m_FrequencyGain = shakeFrequency;
        }
        if (chromaticAberration != null)
            chromaticAberration.intensity.value = 1f;

        // Sonido de Harold Break
        if (haroldBreakSound != null)
           SFXManager.Instance.PlaySFX(haroldBreakSound);

        // loop en simultaneo de zoom y shake
        while (shakeTimer < shakeDuration)
        {
                InputManager.Instance.LockDialogueInputs();
            // Zoom progresivo
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

            // Contador de shake
            shakeTimer += Time.deltaTime;
        

        if (chromaticAberration != null)
        {
                // Normalizar el tiempo (0 a 1)
                float t = shakeTimer / shakeDuration;

                // Curva de intensidad: sube suave, se mantiene, baja suave
                // SmoothStep hace que el inicio y final sean progresivos
                chromaIntensity = Mathf.SmoothStep(0f, 1f, t < 0.5f ? t * 2f : 2f - 2f * t);

                chromaticAberration.intensity.value = chromaIntensity;
            }
        yield return null; 
    }
        

         InputManager.Instance.UnlockDialogueInputs();
        // Resetear shake y aberración
        if (perlinNoise != null)
        {
            perlinNoise.m_AmplitudeGain = 0f;
            perlinNoise.m_FrequencyGain = 0f;
        }
        // Asegurar zoom final
       // virtualCamera.m_Lens.OrthographicSize = targetZoom;

        haroldBreak = false;
        // Disparar cinematica
        if (cinematicController != null)
        {
            yield return new WaitForSeconds(0.5f); // pequeño delay para que no corte el zoom
            cinematicController.PlayCinematic();
        }

    }

    void Update()
    {
        // Zoom normal (sin Harold Break)
        if (zooming)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

            if (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - targetZoom) < 0.05f)
            {
                virtualCamera.m_Lens.OrthographicSize = targetZoom;
                zooming = false;
            }
        }
        else if (resetting)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, originalZoom, Time.deltaTime * zoomSpeed);

            if (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - originalZoom) < 0.05f)
            {
                virtualCamera.m_Lens.OrthographicSize = originalZoom;
                resetting = false;
            }
        }
    }

    public void ResetZoom()
    {
        resetting = true;
        zooming = false;
    }
}
