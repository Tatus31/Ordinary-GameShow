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

    private void Awake()
    {
        if (!uiCamera || !canvasRect || !hoverObject)
            Debug.LogWarning("Missing references in AxeRaycast");
    }

    private void Update()
    {
        Vector3? hitPoint = GetMouseHitPointOnCanvas();

        if (hitPoint.HasValue)
        {
            hoverObject.transform.position = hitPoint.Value + canvasRect.forward * distanceFromCanvas;
            //hoverObject.transform.rotation = Quaternion.LookRotation(canvasRect.forward, Vector3.up);

            CheckObjectsUnderMouse(Input.mousePosition, "Target");
            
            if(Input.GetButtonDown("Fire1"))
            {
                Vector3 axeHitPoint = hitPoint.Value + canvasRect.forward * distanceFromCanvas;
                StartCoroutine(CheckIfTargetHitAfterDuration(0.5f, axeHitPoint));
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

    private void CheckObjectsUnderMouse(Vector2 screenPos, string targetTag)
    {
        foreach (RectTransform child in canvasRect)
        {
            if (child.gameObject.CompareTag(targetTag))
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(child, screenPos, uiCamera))
                {
                    Debug.Log("axe is over object with tag: " + targetTag);
                }
            }
        }
    }

    private IEnumerator CheckIfTargetHitAfterDuration(float duration, Vector3 axeHitPoint)
    {
        yield return new WaitForSeconds(duration);

        Vector3 viewportPos = uiCamera.WorldToViewportPoint(axeHitPoint);
        Vector2 axeHitScreenPos = uiCamera.ViewportToScreenPoint(viewportPos);

        CheckObjectsUnderMouse(axeHitScreenPos, "Target");

        Instantiate(axeHitObject, axeHitPoint, Quaternion.identity, canvasRect.transform);
    }

}
