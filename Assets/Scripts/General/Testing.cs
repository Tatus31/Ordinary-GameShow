using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneController.Instance.LoadSceneWithTransition();
        }
    }
}
