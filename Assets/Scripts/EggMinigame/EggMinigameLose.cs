using System;
using UnityEngine;

public class EggMinigameLose : MonoBehaviour
{
    [SerializeField] int maxEggsDestroyed = 5;
    [SerializeField] string nextScene;
    [SerializeField] EggSpawner eggSpawner;
    
    private bool _hasFoundSceneController;
    
    private void Start()
    {
        _hasFoundSceneController  = false;   
        EggHitsGround.OnEggDestroyed += EggHitsGround_OnEggDestroyed;
    }

    private void EggHitsGround_OnEggDestroyed(int eggsDestroyed)
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

        if (eggsDestroyed >= maxEggsDestroyed)
        {
            eggSpawner.DestroyAllEggs();
            SceneController.Instance.PrewarmScene(nextScene);
            SceneController.Instance.LoadSceneWithTransition();
        }
    }


    private void OnDestroy()
    {
        EggHitsGround.OnEggDestroyed -= EggHitsGround_OnEggDestroyed;
    }
}
