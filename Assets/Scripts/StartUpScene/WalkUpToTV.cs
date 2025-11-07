using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class WalkUpToTV : MonoBehaviour
{
    public static Action OnCamerasComplete;
    
    [Serializable]
    public class CameraSettings
    {
        public CinemachineCamera Camera;
        public float WaitTime = 1f;
    }
    
    [SerializeField] private CinemachineBlenderSettings blenderSettings;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CameraSettings[] cameras;

    private void Awake()
    {
        if (cameras.Length == 0)
        {
            Debug.LogWarning("WalkUpToTV: cameras is null");
        }

        if (!blenderSettings)
        {
            Debug.LogWarning("WalkUpToTV: blenderSettings is null");
        }
        
        if (!cinemachineBrain)
        {
            Debug.LogWarning("`WalkUpToTV`: cinemachineBrain is null");
        }

        foreach (var camera in cameras)
        {
            camera.Camera.enabled = false;
        }
    }

    private void Start()
    {
        SceneController.Instance.PrewarmScene("GameShowScene");
        
        StartCoroutine(ActivateCamerasInSequence());
    }
    
    private IEnumerator ActivateCamerasInSequence()
    {
        cameras[0].Camera.enabled = true;
        yield return new WaitForSeconds(cameras[0].WaitTime);

        for (int i = 1; i < cameras.Length; i++)
        {
            yield return new WaitForSeconds(cameras[i].WaitTime);
            
            cameras[i - 1].Camera.enabled = false;
            cameras[i].Camera.enabled = true;
        }
        
        if (cinemachineBrain)
        {
            yield return new WaitWhile(() => cinemachineBrain.IsBlending);
        }

        CursorState.UnlockCursor();
        OnCamerasComplete?.Invoke();
    }
}
