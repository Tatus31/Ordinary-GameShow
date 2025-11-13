using UnityEngine;
using UnityEngine.Audio;

public class ChangeAudioSetting : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    public void ChangeMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", volume);
    }
}
