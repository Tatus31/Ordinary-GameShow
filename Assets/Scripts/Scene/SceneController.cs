using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LastScene
{
    GameShowScene,
    EggMinigameScene,
    AxeThrowingMinigameScene,
    WacamoleMInigameScene,
    QuizMinigameScene,
    WhereIsWaldoMinigame,
    GameShowSceneAfterAxeMinigame,
    GameShowSceneAfterEggMinigame,
    GameShowSceneAfterQuizMinigame,
    GameShowSceneAfterWacamoleMinigame
}

public class SceneController : MonoBehaviour
{   
    [SerializeField] private GameObject curtainObj;
    [SerializeField] private Animator  animator;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private bool shouldPersist;
    
    public static LastScene LastScene;
    
    public static SceneController Instance;

    private AsyncOperation _loadSceneOperation;
    private string _currentPrewarmedScene;

    public void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
#if UNITY_EDITOR
            Debug.LogWarning("Duplicate instance of SceneController");    
#endif
            return;
        }

        Instance = this;
        
        if(shouldPersist)
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
#if UNITY_EDITOR
        Debug.Log($"OnSceneLoaded: {scene.name}, _loadSceneOperation is {(_loadSceneOperation == null ? "null" : "not null")}");
#endif
        
        if (_loadSceneOperation != null && _loadSceneOperation.isDone)
        {
#if UNITY_EDITOR
            Debug.Log($"Clearing _loadSceneOperation after loading {scene.name}");
#endif
            _loadSceneOperation = null;
            _currentPrewarmedScene = null;
        }
    }

    public void PrewarmScene(string sceneName)
    {
        if (_loadSceneOperation != null)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Another scene is being prewarmed: {_currentPrewarmedScene}. Current request: {sceneName}");
#endif
            return;
        }
#if UNITY_EDITOR
        Debug.Log("Prewarming scene: " + sceneName);
#endif
        _currentPrewarmedScene = sceneName;
        StartCoroutine(PrewarmSceneCoroutine(sceneName));
    }

    public void CancelPrewarm()
    {
#if UNITY_EDITOR
        Debug.Log("Cancelling prewarm operation");
#endif
        _loadSceneOperation = null;
        _currentPrewarmedScene = null;
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
        if (_loadSceneOperation == null)
        {
#if UNITY_EDITOR
            Debug.LogError("No prewarmed scene to load! Call PrewarmScene() first.");
#endif
            return;
        }
        
        Scene currentScene = SceneManager.GetActiveScene();
        
        try
        {
            LastScene = (LastScene)Enum.Parse(typeof(LastScene), currentScene.name);
        }
        catch
        {
#if UNITY_EDITOR
            Debug.LogWarning("Current scene is not in LastScene enum: " + currentScene.name);
#endif
        }
        
        curtainObj.SetActive(true);
        StartCoroutine(LoadSceneWithTransitionCoroutine());
    }

    public void LoadSceneWithPrewarm(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
#if UNITY_EDITOR
            Debug.LogError("[SceneController] sceneName is null or empty!");
#endif
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
#if UNITY_EDITOR
            Debug.LogError($"Scene {sceneName} cannot be loaded. Check Build Settings or scene name spelling.");
#endif
            return;
        }

        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
#if UNITY_EDITOR      
            Debug.LogWarning($"[SceneController] Scene '{sceneName}' is already loaded. Skipping load.");
#endif
            return;
        }

        if (_loadSceneOperation != null)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Clearing existing prewarm ({_currentPrewarmedScene}) to load {sceneName}");
#endif
            _loadSceneOperation = null;
            _currentPrewarmedScene = null;
        }

        StartCoroutine(LoadSceneWithPrewarmCoroutine(sceneName));
    }


    private IEnumerator LoadSceneWithPrewarmCoroutine(string sceneName)
    {
        _currentPrewarmedScene = sceneName;
        _loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _loadSceneOperation.allowSceneActivation  = false;

        while (_loadSceneOperation.progress < 0.9f)
        {
            yield return null;
        }

        curtainObj.SetActive(true);
        animator.SetBool("StartTransition", true);
        yield return new WaitForSeconds(transitionDuration);
        
        _loadSceneOperation.allowSceneActivation = true;
        yield return _loadSceneOperation;
        
        animator.SetBool("StartTransition", false);
        yield return new WaitForSeconds(transitionDuration);
        curtainObj.SetActive(false);
        
        _loadSceneOperation = null;
        _currentPrewarmedScene = null;
    }

    private IEnumerator LoadSceneWithTransitionCoroutine()
    {
        animator.SetBool("StartTransition", true);

        yield return new WaitForSeconds(transitionDuration);
        
        if (_loadSceneOperation == null)
        {
#if UNITY_EDITOR
            Debug.LogError("loadSceneOperation is null during transition");
#endif
            animator.SetBool("StartTransition", false);
            curtainObj.SetActive(false);
            yield break;
        }
        
        _loadSceneOperation.allowSceneActivation  = true; 
        
        yield return _loadSceneOperation;
        
        animator.SetBool("StartTransition", false);
        
        yield return new WaitForSeconds(transitionDuration);
        
        curtainObj.SetActive(false);
        
        _loadSceneOperation = null;
        _currentPrewarmedScene = null;
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
#if UNITY_EDITOR
            Debug.Log($"Scene {sceneName} prewarmed and ready");
#endif
        }
    }
}