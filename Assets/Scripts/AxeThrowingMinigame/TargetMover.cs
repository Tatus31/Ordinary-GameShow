using System.Collections;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform target;
    [SerializeField] private RectTransform canvasRect;

    [Header("Movement Settings")]
    [SerializeField] private Vector2 startPoint;
    [SerializeField] private Vector2 endPoint;
    [SerializeField] private float duration = 2f; 

    private void Awake()
    {
        if (!target)
            Debug.LogWarning("Target is not assigned in TargetMover");
    }

    private void Start()
    {
        target = Instantiate(target, startPoint, Quaternion.identity, canvasRect.transform);
        
        if (target)
        {
            target.anchoredPosition = startPoint;
            StartCoroutine(MoveTarget());
        }
    }

    private IEnumerator MoveTarget()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float time = Mathf.Clamp01(elapsedTime / duration);
            target.anchoredPosition = Vector2.Lerp(startPoint, endPoint, time);

            yield return null;
        }

        target.anchoredPosition = endPoint;
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
