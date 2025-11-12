using System;
using Unity.VisualScripting;
using UnityEngine;

public class PointerFindWaldo : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private float minPointDivision;
    [SerializeField] private float maxPointDivision;
    
    private bool _isRightAnswer = false;
    private bool _hasFoundSceneController;

    private float _timeFindingWaldo;

    private float dividedValue;
    
    private void Start()
    {
        FindWaldoTimer.OnStartTransition += FindWaldoTimer_OnStartTransition;
    }

    private void FindWaldoTimer_OnStartTransition(int obj)
    {
        _timeFindingWaldo = obj; 
        
        LoadNextScene();
        _isRightAnswer  = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Waldo"))
        {
            _isRightAnswer  = true;
        }
    }

    private void Update()
    {
        if (_isRightAnswer && Input.GetButtonDown("Fire1"))
        {
            FindWaldoTimer.HasFoundWaldo = true;
            AudioManager.PlaySound("FoundVHS_Cheer");
            _isRightAnswer  = false;
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (!SceneController.Instance)
        {
#if UNITY_EDITOR
            Debug.LogError("No SceneController found in the scene.");     
#endif
            return;
        }

        if (PointManager.Instance.CurrentAllMinigamePoints != 0)
        {
            float t = Mathf.Clamp01(_timeFindingWaldo);
            _timeFindingWaldo = Mathf.Lerp(minPointDivision, maxPointDivision, t);

            int value = Mathf.FloorToInt(_timeFindingWaldo);
            if(value != 0)
                PointManager.Instance.CurrentAllMinigamePoints /= value;
        }


        SceneController.Instance.LoadSceneWithPrewarm(sceneName);
    }

    private void OnDestroy()
    {
        FindWaldoTimer.OnStartTransition -= FindWaldoTimer_OnStartTransition;
    }
}
