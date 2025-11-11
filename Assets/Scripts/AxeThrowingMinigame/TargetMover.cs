using System;
using System.Collections;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public static event System.Action<int> OnTargetEscaped;
    
    [Header("References")]
    [SerializeField] private RectTransform targetPrefab;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private RectTransform targetContainerRect;

    [Header("Settings")]
    [SerializeField] private Vector2 startPoint;
    [SerializeField] private Vector2 endPoint;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float scaleDuration = 0.5f;
    [Header("Speed Settings")]
    [SerializeField] private float moveDurationDecrease =  0.3f;
    [SerializeField] private float scaleDurationDecrease =  0.1f;
    [SerializeField] private float minMoveDuration = 0.5f;
    [SerializeField] private float minScaleDuration = 0.3f;

    private readonly Vector3 _smallScale = Vector3.one * 0.1f;
    private readonly Vector3 _bigScale = Vector3.one;
    
    private float _currentMoveDuration;
    private float _currentScaleDuration;
    
    private static float GlobalMoveDurationBonus = 0f;
    private static float GlobalScaleDurationBonus = 0f;
    
    private static int EscapedTargetCount = 0;

    private void Start()
    {
        EscapedTargetCount = 0;
    }

    public void StartMoving()
    {
        _currentMoveDuration = Mathf.Max(minMoveDuration, moveDuration - GlobalMoveDurationBonus);
        _currentScaleDuration = Mathf.Max(minScaleDuration, scaleDuration - GlobalScaleDurationBonus);
        
        Debug.Log("Next target durations => Move: " + _currentMoveDuration + ", Scale: " + _currentScaleDuration);
        Debug.Log($"Next target globalMoveDurationBonus => {GlobalMoveDurationBonus} , ScaleDurationBonus => {GlobalScaleDurationBonus}");
        
        if(this)
            StartCoroutine(SpawnAndAnimateTarget());
    }

    private IEnumerator SpawnAndAnimateTarget()
    {
        RectTransform targetInstance = Instantiate(targetPrefab, targetContainerRect);
        targetInstance.anchoredPosition = startPoint;
        targetInstance.localScale = _smallScale;
        targetInstance.gameObject.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < _currentScaleDuration)
        {
            if (!targetInstance)
                yield break; 
            
            elapsedTime += Time.deltaTime;
            float time = Mathf.Clamp01(elapsedTime / _currentScaleDuration);
            targetInstance.localScale = Vector3.Lerp(_smallScale, _bigScale, time);
            
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < _currentMoveDuration)
        {
            if (!targetInstance) 
                yield break;
            
            elapsedTime += Time.deltaTime;
            float time = Mathf.Clamp01(elapsedTime / _currentMoveDuration);
            targetInstance.anchoredPosition = Vector2.Lerp(startPoint, endPoint, time);
            
            yield return null;
        }

        if (!targetInstance) 
            yield break;
        
        targetInstance.anchoredPosition = endPoint;

        elapsedTime = 0f;
        while (elapsedTime < _currentScaleDuration)
        {
            if (!targetInstance)
                yield break; 
            
            elapsedTime += Time.deltaTime;
            float time = Mathf.Clamp01(elapsedTime / _currentScaleDuration);
            targetInstance.localScale = Vector3.Lerp(_bigScale, _smallScale, time);
            
            yield return null;
        }

        if (targetInstance)
        {
            if (!targetInstance.CompareTag("Target_Red"))
            {
                EscapedTargetCount++;
                OnTargetEscaped?.Invoke(EscapedTargetCount);
                XActivation.Instance.ActivateX();
            }
            
            Destroy(targetInstance.gameObject);
        } 
    }


    public void SpeedUpTarget()
    {
        GlobalMoveDurationBonus += moveDurationDecrease;
        GlobalScaleDurationBonus += scaleDurationDecrease;
    }
    
    private void OnDrawGizmos()
    {
        if (!canvasRect) 
            return;

        Vector3 startWorld = canvasRect.TransformPoint(startPoint);
        Vector3 endWorld = canvasRect.TransformPoint(endPoint);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startWorld, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endWorld, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startWorld, endWorld);
    }
}
