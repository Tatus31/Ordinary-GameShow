using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{   
    public static SceneController Instance;
    
    private AsyncOperation _loadSceneOperation;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
#if UNITY_EDITOR
            Debug.LogWarning("Instance already destroyed");
#endif
        }
    }

    public void PrewarmScene(string sceneName)
    {
        if (_loadSceneOperation != null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("other scene is beeing prewarmed");
#endif
            return;
        }

        StartCoroutine(PrewarmSceneCoroutine(sceneName));
    }

    public void LoadScene(string sceneName)
    {
        if (_loadSceneOperation == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("_loadSceneOperation is null");
#endif
            return;
        }
        
        _loadSceneOperation.allowSceneActivation  = true; 
    }

    private IEnumerator PrewarmSceneCoroutine(string sceneName)
    {
        _loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _loadSceneOperation.allowSceneActivation = false;

        while (_loadSceneOperation.progress < 0.9f)
        {
            yield return null;
        }
#if UNITY_EDITOR
        Debug.Log("Prewarmed scene " + sceneName);
#endif
    }
}
