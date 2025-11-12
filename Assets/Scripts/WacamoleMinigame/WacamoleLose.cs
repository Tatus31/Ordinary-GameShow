using System;
using UnityEngine;

public class WacamoleLose : MonoBehaviour
{
    [SerializeField] private int maxEscapedDucks;
    [SerializeField] string nextScene;
    [SerializeField] MoleAreaManager moleAreaManager;
    
    private bool _hasFoundSceneController;
    
    private void Start()
    {
        MoleAreaManager.OnDuckEscaped += MoleAreaManager_OnDuckEscaped;
    }

    private void MoleAreaManager_OnDuckEscaped(int escapedDucks)
    {
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
        
        if (escapedDucks >= maxEscapedDucks)
        {
            moleAreaManager.DestroyAllDucks();
            SceneController.Instance.PrewarmScene(nextScene);
            SceneController.Instance.LoadSceneWithTransition();
        }
    }
}
