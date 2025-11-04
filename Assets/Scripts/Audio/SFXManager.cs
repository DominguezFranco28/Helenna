using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [System.Serializable]
    public class EmotionAudio //para los dialogos
    {
        public string token;
        public AudioClip clip;
    }

    public List<EmotionAudio> emotionAudios;
    private Dictionary<string, AudioClip> _emotionDict;
    //Singleton
    public static SFXManager Instance { get; private set; }
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _loopSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _ambienceSource;
    [SerializeField] private AudioSource _voice;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevents duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes. 

        if (_loopSource == null)
        {
            Debug.LogError("SFXPlayerController needs an AudioSource.");
            enabled = false;
        }
        // Llenar diccionario para acceso rápido
        _emotionDict = new Dictionary<string, AudioClip>();
        foreach (var emotion in emotionAudios)
        {
            if (emotion.clip != null && !string.IsNullOrEmpty(emotion.token))
                _emotionDict[emotion.token.ToLower().Trim()] = emotion.clip;
        }
        RefreshEmotionDict();

    }
    [ContextMenu("Refresh Emotion Dictionary")]
    private void RefreshEmotionDict()
    {
        _emotionDict = new Dictionary<string, AudioClip>();
        foreach (var emotion in emotionAudios)
        {
            if (emotion.clip != null && !string.IsNullOrEmpty(emotion.token))
                _emotionDict[emotion.token.ToLower().Trim()] = emotion.clip;
        }
    }
    public void PlayAmbience(AudioClip audioClip)
    {
       if (_ambienceSource != null && audioClip != null)
        {
            if (_ambienceSource.isPlaying)
            {
                _ambienceSource.Stop();
            }
            _ambienceSource.clip = audioClip;
            _ambienceSource.Play();
        }
    }
    public void PlayEmotion(string token)
    {
        if (string.IsNullOrEmpty(token)) return;

        token = token.ToLower().Trim();
        if (_emotionDict.TryGetValue(token, out AudioClip clip))
        {
            _voice.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"No se encontró audio para token '{token}'");
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
        if (_loopSource != null && clip != null)
        {
            _loopSource.clip = clip;
            _loopSource.loop = true;
            _loopSource.Play();
        }
    }

    //Stop the loop.
    public void StopLoop()
    {
        if (_loopSource != null)
        {
            _loopSource.Stop();
        }
    }
    [ContextMenu("Setup Default Emotions")]
    private void SetupDefaultEmotions()
    {
        emotionAudios = new List<EmotionAudio>()
    {
        // Harold
        new EmotionAudio() { token = "harold-annoyed", clip = null },
        new EmotionAudio() { token = "harold-doubt", clip = null },
        new EmotionAudio() { token = "harold-demand", clip = null },
        new EmotionAudio() { token = "harold-mocking", clip = null },
        new EmotionAudio() { token = "harold-happy", clip = null },
        new EmotionAudio() { token = "harold-sad", clip = null },

        // Rex
        new EmotionAudio() { token = "rex-barking", clip = null },
        new EmotionAudio() { token = "rex-growling", clip = null },
        new EmotionAudio() { token = "rex-whining", clip = null },

        // Nina
        new EmotionAudio() { token = "nina-annoyed", clip = null },
        new EmotionAudio() { token = "nina-doubt", clip = null },
        new EmotionAudio() { token = "nina-demand", clip = null },
        new EmotionAudio() { token = "nina-mocking", clip = null },
        new EmotionAudio() { token = "nina-happy", clip = null },
        new EmotionAudio() { token = "nina-sad", clip = null }
    };
    }
}

