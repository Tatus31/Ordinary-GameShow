using UnityEngine;

public class ChangeSettingType : MonoBehaviour
{
    [SerializeField] private RectTransform audioRect;
    [SerializeField] private RectTransform resolutionRect;
    
    public void ChangeToAudio()
    {
        resolutionRect.gameObject.SetActive(false);
        audioRect.gameObject.SetActive(true);
    }

    public void ChangeToResolution()
    {
        audioRect.gameObject.SetActive(false);
        resolutionRect.gameObject.SetActive(true);
    }
}
