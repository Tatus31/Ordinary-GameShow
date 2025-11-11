using System;
using Unity.VisualScripting;
using UnityEngine;

public class PointerFindWaldo : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    private bool isRightAnswer = false;

    private void Start()
    {
        if (SceneController.Instance)
            SceneController.Instance.PrewarmScene(sceneName);
        else
            Debug.Log("there is no SceneController.Instance");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Waldo"))
        {
            isRightAnswer  = true;
            Debug.Log("Waldo");
        }
    }

    private void Update()
    {
        if (isRightAnswer && Input.GetButtonDown("Fire1"))
        {
            AudioManager.PlaySound("FoundVHS_Cheer");
            isRightAnswer  = false;
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (SceneController.Instance)
        {
            SceneController.Instance.LoadSceneWithTransition();
        }
    }
}
