using System;
using UnityEngine;
using UnityEngine.UI;

public class XActivation : MonoBehaviour
{
    public static XActivation Instance;
    
    [SerializeField] private RectTransform[] xes;
    [SerializeField] private Sprite xCheckedSprite;
    [SerializeField] private Sprite xUncheckedSprite;

    private int currentX = 0;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        DeactivateX();
    }

    public void ActivateX()
    {
        if (xes.Length > currentX)
        {
            xes[currentX].GetComponent<Image>().sprite = xCheckedSprite;
            currentX++;
        }

    }

    public void DeactivateX()
    {
        foreach (RectTransform r in xes)
        {
            r.GetComponent<Image>().sprite = xUncheckedSprite;
        }

        currentX = 0;
    }
}
