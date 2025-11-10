using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Needed if using UI elements

[RequireComponent(typeof(Canvas))]
public class AxeRaycast : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera uiCamera;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameObject hoverObject;
    [SerializeField] private GameObject axeHitObject;

    [Header("Settings")]
    [SerializeField] private float distanceFromCanvas = 0.01f;

    [Header("Points")] [SerializeField] private int points;
    
    private void Awake()
    {
        if (!uiCamera || !canvasRect || !hoverObject)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Missing references in AxeRaycast");
#endif
        }
    }

    private void Update()
    {
        Vector3? hitPoint = GetMouseHitPointOnCanvas();

        if (hitPoint.HasValue)
        {
            hoverObject.transform.position = hitPoint.Value + canvasRect.forward * distanceFromCanvas;

            if (Input.GetButtonDown("Fire1"))
            {
                GameObject hitObject = GetObjectUnderMouse(canvasRect,Input.mousePosition, "Target");
                
                if (hitObject)
                {
                    StartCoroutine(HitCooldown(0.3f, hitObject));
                }
            }
        }
    }

    private Vector3? GetMouseHitPointOnCanvas()
    {
        if (!uiCamera || !canvasRect)
            return null;

        Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
        Plane canvasPlane = new Plane(canvasRect.forward, canvasRect.position);

        if (canvasPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, uiCamera, out Vector2 localPoint))
            {
                if (canvasRect.rect.Contains(localPoint))
                    return hitPoint;
            }
        }

        return null;
    }

    private GameObject GetObjectUnderMouse(Transform parent, Vector2 screenPos, string targetTag)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.CompareTag(targetTag) && RectTransformUtility.RectangleContainsScreenPoint(child as RectTransform, screenPos, uiCamera))
            {
                return child.gameObject;
            }

            GameObject found = GetObjectUnderMouse(child, screenPos, targetTag);
            
            if (found) 
                return found;
        }

        return null;
    }

    
    private IEnumerator HitCooldown(float seconds, GameObject hitObject)
    {
        yield return new WaitForSeconds(seconds);
        
        if (hitObject)
        {
            Debug.Log("Hit target");
            Destroy(hitObject);
            PointManager.Instance.AddPoints(points);
        }
    }
}
