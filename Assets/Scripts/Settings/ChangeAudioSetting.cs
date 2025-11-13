using UnityEngine;
using UnityEngine.Audio;

public class ChangeAudioSetting : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    public void ChangeMasterVolume(float volume)
    {
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        Debug.Log(volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void ChangeSFXVolume(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void ChangeVoiceVolume(float volume)
    {
        mixer.SetFloat("Voice", Mathf.Log10(volume) * 20);
    }
}
