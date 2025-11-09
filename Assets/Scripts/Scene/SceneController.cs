using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{   
    [SerializeField] private GameObject curtainObj;
    [SerializeField] private Animator  animator;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private bool shouldPersist;
    
    public static SceneController Instance;

    private AsyncOperation _loadSceneOperation;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            if(shouldPersist)
                DontDestroyOnLoad(gameObject);
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

    public void LoadScene()
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

    public void LoadSceneWithTransition()
    {
        curtainObj.SetActive(true);
        StartCoroutine(LoadSceneWithTransitionCoroutine());
    }

    private IEnumerator LoadSceneWithTransitionCoroutine()
    {
        animator.SetBool("StartTransition", true);

        yield return new WaitForSeconds(transitionDuration);
        
        _loadSceneOperation.allowSceneActivation  = true; 
        
        yield return _loadSceneOperation;
        
        animator.SetBool("StartTransition", false);
        
        yield return new WaitForSeconds(transitionDuration);
        
        curtainObj.SetActive(false);
    }
    
    private IEnumerator PrewarmSceneCoroutine(string sceneName)
    {
        _loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        
        if (_loadSceneOperation != null)
        {
            _loadSceneOperation.allowSceneActivation = false;

            while (_loadSceneOperation.progress < 0.9f)
            {
                yield return null;
            }
        }

#if UNITY_EDITOR
        Debug.Log("Prewarmed scene " + sceneName);
#endif
    }
}
