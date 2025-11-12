using UnityEngine;

public class SceneControllerProxyBehaviour : MonoBehaviour
{
    public void PrewarmScene(string sceneName)
    {
        SceneControllerProxy.PrewarmScene(sceneName);
    }

    public void LoadScene()
    {
        SceneControllerProxy.LoadScene();
    }

    public void LoadSceneWithTransition()
    {
        SceneControllerProxy.LoadSceneWithTransition();
    }

    public void LoadSceneWithPrewarm(string sceneName)
    {
        SceneControllerProxy.LoadSceneWithPrewarm(sceneName);
    }
}