using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    public static AudioManager Instance => _instance;
    private static AudioManager _instance;

   
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public float GetMixerGroupVolume(string mixer)
    {
        var groups = audioMixer.FindMatchingGroups(mixer);
        if (groups != null && groups.Length > 0)
        {
            float db = 0f;
            bool worked = audioMixer.GetFloat(mixer, out db);

            if (!worked) Debug.Log("Error getting mixer group volume: "+ mixer +" results: "+ groups.Length);

            return Mathf.Pow(10f, db / 20f);
        }
        else
        {
            Debug.Log("Group not found: " + mixer);
            return 0f;
        }
    }

    public void SetMixerGroupVolume(string mixer, float volume)
    {
        var groups = audioMixer.FindMatchingGroups(mixer);
        if (groups != null && groups.Length > 0)
        {
            // Group exists, set volume (convert from 0–1 range to decibels if needed)
            float db = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            Debug.Log("Slider moved, Vol: " + volume + " Db: " + db);
            bool worked = audioMixer.SetFloat(mixer, db);

            if (!worked) Debug.Log("Error setting mixer group volume");
        }
        else
        {
            Debug.Log("Group not found: " + mixer);
        }
    }
}
