using System;
using UnityEngine;

public class AxeThrowingLose : MonoBehaviour
{
    [SerializeField] int maxTargetsEscaped = 5;
    [SerializeField] string nextScene;
    [SerializeField] TargetSpawnManager targetSpawnManager;
    
    private bool _hasFoundSceneController;
    
    private void Start()
    {
        TargetMover.OnTargetEscaped += TargetMover_OnTargetEscaped;
    }

    private void TargetMover_OnTargetEscaped(int escapedTargets)
    {
        Debug.Log(escapedTargets);
        
        if (!_hasFoundSceneController)
        {
            if (!SceneController.Instance)
            {
                SceneController.Instance = FindFirstObjectByType<SceneController>();
                if (!SceneController.Instance)
                {
#if UNITY_EDITOR
                    Debug.LogError("No SceneController found in the scene.");
#endif
                    return;
                }
            }

            _hasFoundSceneController = true;
        }

        if (escapedTargets >= maxTargetsEscaped)
        {
            targetSpawnManager.DestroyAllTargets();
            SceneController.Instance.PrewarmScene(nextScene);
            SceneController.Instance.LoadSceneWithTransition();
        }
    }
    
    private void OnDestroy()
    {
        EggHitsGround.OnEggDestroyed -= TargetMover_OnTargetEscaped;
    }
}
