using System;
using UnityEngine;

public class AudianceManager : MonoBehaviour
{
    [SerializeField] private GameObject[] audiance;
    [Header("Audiance sprites")]
    [SerializeField] private Sprite audianceNeutralSprite; 
    [SerializeField] private Sprite audianceCheerSprite;
    [SerializeField] private Sprite audianceBooSprite;
    
    private SpriteRenderer[] _audianceSprites;
    private void Awake()
    {
        _audianceSprites = new  SpriteRenderer[audiance.Length];

        for (int i = 0; i < audiance.Length; i++)
        {
            _audianceSprites[i] = audiance[i].GetComponent<SpriteRenderer>();
        }
    }

    public void SetAudianceToNeutral()
    {
        ChangeAllSprites(audianceNeutralSprite);
    }

    public void SetAudianceToCheer()
    {
        ChangeAllSprites(audianceCheerSprite);
    }

    public void SetAudianceToBoo()
    {
        ChangeAllSprites(audianceBooSprite);
    }

    private void ChangeAllSprites(Sprite sprite)
    {
        foreach (SpriteRenderer spriteRenderer in _audianceSprites)
        {
            if (spriteRenderer)
            {
                spriteRenderer.sprite = sprite;
            }
        }
    }
}
