using UnityEngine;

public class ChangeSettingType : MonoBehaviour
{
    [SerializeField] private RectTransform audio;
    [SerializeField] private RectTransform resolution;
    
    public void ChangeToAudio()
    {
        resolution.gameObject.SetActive(false);
        audio.gameObject.SetActive(true);
    }

    public void ChangeToResolution()
    {
        audio.gameObject.SetActive(false);
        resolution.gameObject.SetActive(true);
    }
}
