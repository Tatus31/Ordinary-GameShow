using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PointerFindWaldo : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private float minPointDivision;
    [SerializeField] private float maxPointDivision;
    
    private bool _isRightAnswer = false;
    private bool _hasFoundSceneController;
    private bool _canStart;

    private float _timeFindingWaldo;

    private float _dividedValue;
    
    private void Start()
    {
        FindWaldoTimer.OnStartTransition += FindWaldoTimer_OnStartTransition;
        StartCoroutine(TimerBeforeSearchBegins());
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
        if(!_canStart)
            return;
        
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
    
    private IEnumerator TimerBeforeSearchBegins()
    {
        _canStart = false;
        yield return new WaitForSeconds(1f);
        _canStart = true;
    }

    private void OnDestroy()
    {
        FindWaldoTimer.OnStartTransition -= FindWaldoTimer_OnStartTransition;
    }
    
}
