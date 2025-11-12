using System;
using Unity.VisualScripting;
using UnityEngine;

public class PointerFindWaldo : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    private bool isRightAnswer = false;
    private bool _hasFoundSceneController;
    
    private void Start()
    {

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
        if (!SceneController.Instance)
        {
            Debug.LogError("No SceneController found in the scene.");
            return;
        }

        SceneController.Instance.LoadSceneWithPrewarm(sceneName);
    }
}
