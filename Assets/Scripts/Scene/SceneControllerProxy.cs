using UnityEngine;

public static class SceneControllerProxy
{
    public static void PrewarmScene(string sceneName)
    {
        if (SceneController.Instance)
            SceneController.Instance.PrewarmScene(sceneName);
        else
            Debug.LogWarning("No active SceneController instance found when calling PrewarmScene.");
    }
    
    public static void LoadScene()
    {
        if (SceneController.Instance)
            SceneController.Instance.LoadScene();
        else
            Debug.LogWarning("No active SceneController instance found when calling LoadScene.");
    }
    
    public static void LoadSceneWithTransition()
    {
        if (SceneController.Instance)
            SceneController.Instance.LoadSceneWithTransition();
        else
            Debug.LogWarning("No active SceneController instance found when calling LoadSceneWithTransition.");
    }
    
    public static void LoadSceneWithPrewarm(string sceneName)
    {
        if (SceneController.Instance)
            SceneController.Instance.LoadSceneWithPrewarm(sceneName);
        else
            Debug.LogWarning("No active SceneController instance found when calling LoadSceneWithPrewarm.");
    }

    public static void CancelPrewarm()
    {
        if (SceneController.Instance)
            SceneController.Instance.CancelPrewarm();
        else
            Debug.LogWarning("No active SceneController instance found when calling CancelPrewarm.");
    }
}