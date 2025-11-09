using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Unmute & Fade Settings (For UnityEvents)")]
    [SerializeField] private string soundToControl;
    [SerializeField] private float playDuration = 4f;
    [SerializeField] private float fadeDuration = 4f;

    [SerializeField] private List<AudioSource> playingSounds = new List<AudioSource>();

    private void Awake()
    {
        if (Instance && Instance != this)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Multiple instances of AudioManager found. Destroying duplicate on {gameObject.name}");
#endif
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void SetSoundToControl(string soundName)
    {
        soundToControl = soundName;
    }
    
    public void SetPlayDuration(float duration)
    {
        playDuration = duration;
    }

    public void SetFadeDuration(float duration)
    {
        fadeDuration = duration;
    }
    
    public static void PlaySound(string soundName)
    {
        if (!Instance)
        {
#if UNITY_EDITOR
            Debug.LogError("AudioManager instance is null. Cannot play sound.");
#endif
            return;
        }

        StopAllSounds();

        GameObject soundObj = GameObject.Find(soundName);
        
        if (soundObj)
        {
            AudioSource audioSource = soundObj.GetComponent<AudioSource>();
            
            if (audioSource)
            {
                audioSource.mute = false;
                audioSource.Play();
                Instance.playingSounds.Add(audioSource);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"No AudioSource component found on {soundName}");
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Sound object '{soundName}' not found in scene");
#endif
        }
    }

    public static void StopAllSounds()
    {
        if (!Instance) 
            return;

        foreach (AudioSource source in Instance.playingSounds)
        {
            if (source)
            {
                source.mute = true;
            }
        }

        Instance.playingSounds.Clear();
    }

    public static void MuteSound(string soundName, float fadeDuration = 1f)
    {
        if (!Instance) 
            return;

        GameObject soundObj = GameObject.Find(soundName);

        if (soundObj)
        {
            AudioSource audioSource = soundObj.GetComponent<AudioSource>();

            if (audioSource)
            {
                Instance.StartCoroutine(FadeOutAndMute(audioSource, fadeDuration));
            }
        }
    }

    private static IEnumerator FadeOutAndMute(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.mute = true;

        Instance.playingSounds.Remove(audioSource);
    }
    
    public static void UnmuteSound(string soundName)
    {
        if (!Instance) 
            return;
        
        GameObject soundObj = GameObject.Find(soundName);
        
        if (soundObj)
        {
            AudioSource audioSource = soundObj.GetComponent<AudioSource>();
            
            if (audioSource)
            {
                audioSource.mute = false;
                audioSource.volume = 1f;
                if (!Instance.playingSounds.Contains(audioSource))
                {
                    Instance.playingSounds.Add(audioSource);
                }
            }
        }
    }

    public static void ChangeAudioVolume(string soundName, float volume)
    {
        GameObject soundObj = GameObject.Find(soundName);
        
        if (soundObj)
        {
            AudioSource audioSource = soundObj.GetComponent<AudioSource>();
            
            if (audioSource)
            {
                audioSource.volume = Mathf.Clamp01(volume);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"No AudioSource component found on {soundName}");
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Sound object '{soundName}' not found in scene");
#endif
        }
    }

    public static void ChangeAudioPitch(string soundName, float pitch)
    {
        GameObject soundObj = GameObject.Find(soundName);
        
        if (soundObj)
        {
            AudioSource audioSource = soundObj.GetComponent<AudioSource>();
            
            if (audioSource)
            {
                audioSource.pitch = pitch;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"No AudioSource component found on {soundName}");
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Sound object '{soundName}' not found in scene");
#endif
        }
    }

    public static bool IsSoundPlaying(string soundName)
    {
        if (!Instance)
            return false;

        GameObject soundObj = GameObject.Find(soundName);
        
        if (soundObj)
        {
            AudioSource audioSource = soundObj.GetComponent<AudioSource>();
            if (audioSource)
            {
                return audioSource.isPlaying && !audioSource.mute;
            }
        }

        return false;
    }
    
    public void UnmuteThenFadeOut()
    {
        if (!Instance)
            return;

        GameObject soundObj = GameObject.Find(soundToControl);

        if (soundObj)
        {
            AudioSource audioSource = soundObj.GetComponent<AudioSource>();
            if (audioSource)
            {
                StartCoroutine(UnmuteThenFadeOutCoroutine(audioSource, playDuration, fadeDuration));
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning($"No AudioSource component found on {soundToControl}");
            }
#endif
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"Sound object '{soundToControl}' not found in scene");
        }
#endif
    }

    private IEnumerator UnmuteThenFadeOutCoroutine(AudioSource audioSource, float playDuration, float fadeDuration)
    {
        audioSource.mute = false;
        audioSource.volume = 1f;

        if (!audioSource.isPlaying)
            audioSource.Play();

        if (!playingSounds.Contains(audioSource))
            playingSounds.Add(audioSource);

        yield return new WaitForSeconds(playDuration);

        yield return StartCoroutine(FadeOutAndMute(audioSource, fadeDuration));
    }
}