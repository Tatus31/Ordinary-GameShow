using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeResolution : MonoBehaviour
{
    [SerializeField] private Canvas screenCanvas;
    [SerializeField] private Canvas pointCanvas;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private RawImage image;
    [SerializeField] private RectTransform curtains;

    private const string ResolutionIndexKey = "SavedResolutionIndex";
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // if (FindObjectsOfType<ChangeResolution>().Length > 1)
        // {
        //     Destroy(gameObject);
        // }
    }
    private void Start()
    {
        int savedIndex = PlayerPrefs.GetInt(ResolutionIndexKey, 0);

        if (resolutionDropdown != null)
        {
            resolutionDropdown.SetValueWithoutNotify(savedIndex);
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
            
            if (resolutionDropdown.options.Count > 0)
            {
                OnResolutionChanged(savedIndex);
            }
        }
    }

    private void OnResolutionChanged(int index)
    {
        PlayerPrefs.SetInt(ResolutionIndexKey, index);
        PlayerPrefs.Save();

        if (resolutionDropdown == null)
            return;
            
        string resolutionText = resolutionDropdown.options[index].text;
        string[] dimensions = resolutionText.Split('x');
        
        if (dimensions.Length == 2)
        {
            if (int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
            {
                ApplyResolution(width, height);
            }
            else
            {
                Debug.LogError($"Failed to parse resolution: {resolutionText}");
            }
        }
        else
        {
            Debug.LogError($"Invalid resolution format: {resolutionText}. Expected format: WIDTHxHEIGHT");
        }
    }

    private void ApplyResolution(int width, int height)
    {
        float imageWidth = height * (384f / 336f);
        
        if (image != null)
        {
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(imageWidth, height);
        }

        if (curtains != null)
        {
            curtains.sizeDelta = new Vector2(imageWidth, height);
        }

        UpdateCanvasScaler(screenCanvas, width, height);
        UpdateCanvasScaler(pointCanvas, width, height);
        
        Debug.Log($"Resolution changed to: {width}x{height}, RawImage and Curtains size: {imageWidth}x{height}");
    }

    private void UpdateCanvasScaler(Canvas canvas, int width, int height)
    {
        if (canvas != null)
        {
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                scaler.referenceResolution = new Vector2(width, height);
            }
        }
    }

    private void OnDestroy()
    {
        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
        }
    }
}