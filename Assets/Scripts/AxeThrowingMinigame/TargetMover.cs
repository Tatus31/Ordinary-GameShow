using System.Collections;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
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
    
    private static float _globalMoveDurationBonus = 0f;
    private static float _globalScaleDurationBonus = 0f;

    public void StartMoving()
    {
        _currentMoveDuration = Mathf.Max(minMoveDuration, moveDuration - _globalMoveDurationBonus);
        _currentScaleDuration = Mathf.Max(minScaleDuration, scaleDuration - _globalScaleDurationBonus);
        
        Debug.Log("Next target durations => Move: " + _currentMoveDuration + ", Scale: " + _currentScaleDuration);
        Debug.Log($"Next target globalMoveDurationBonus => {_globalMoveDurationBonus} , ScaleDurationBonus => {_globalScaleDurationBonus}");
        
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
            Destroy(targetInstance.gameObject);
    }


    public void SpeedUpTarget()
    {
        _globalMoveDurationBonus += moveDurationDecrease;
        _globalScaleDurationBonus += scaleDurationDecrease;
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
