using System;
using UnityEngine;
using System.Collections;

public class AudianceManager : MonoBehaviour
{
    [SerializeField] private GameObject[] audiance;
    [Space(10)] 
    [SerializeField] private Animator audianceAnimator;
    [SerializeField] private float defaultAnimationTime;
    [Header("Audiance sprites")]
    [SerializeField] private Sprite audianceNeutralSprite; 
    [SerializeField] private Sprite audianceCheerSprite;
    [SerializeField] private Sprite audianceBooSprite;
    
    private SpriteRenderer[] _audianceSprites;
    
    private Coroutine _audianceAnimationCoroutine;
    
    private static readonly int IsCheer = Animator.StringToHash("IsCheer");
    private static readonly int IsNeutral = Animator.StringToHash("IsNeutral");
    private static readonly int IsBoo = Animator.StringToHash("IsBoo");

    private void Awake()
    {
        _audianceSprites = new  SpriteRenderer[audiance.Length];

        for (int i = 0; i < audiance.Length; i++)
        {
            _audianceSprites[i] = audiance[i].GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        if (!audianceAnimator)
        {
#if UNITY_EDITOR
            Debug.LogWarning("There is not audiance animator");
#endif
        }
        else
        {
            audianceAnimator = transform.GetComponent<Animator>();
        }
    }

    public void EvaluateToBooOrToChher(float duration)
    {
        if (PointManager.Instance.IsScorePassable)
        {
            SetAudianceToCheer(duration);
        }
        else
        {
            SetAudianceToBoo(duration);
        }
    }

    public void SetAudianceToNeutral(float duration)
    {
        ChangeAllSprites(audianceNeutralSprite);
        
        if(duration == 0)
            return;
        
        audianceAnimator.SetBool(IsNeutral, true);
        ChangeAudianceAnimation(IsNeutral, false, duration);
    }

    public void SetAudianceToCheer(float duration)
    {
        ChangeAllSprites(audianceCheerSprite);
        
        if(duration == 0)
            return;
        
        audianceAnimator.SetBool(IsCheer, true);
        ChangeAudianceAnimation(IsCheer, false, duration);
    }

    public void SetAudianceToBoo(float duration)
    {
        ChangeAllSprites(audianceBooSprite);
        
        if(duration == 0)
            return;
        
        audianceAnimator.SetBool(IsBoo, true);
        ChangeAudianceAnimation(IsBoo, false, duration);
    }

    private void ChangeAudianceAnimation(int animationHash, bool animationState, float animationTime = 1f)
    {
        if (_audianceAnimationCoroutine != null)
        {
            StopCoroutine(_audianceAnimationCoroutine);
            _audianceAnimationCoroutine = null;
        }
        
        _audianceAnimationCoroutine = StartCoroutine(ChangeAudianceAnimationCoroutine(animationTime,  animationHash, animationState));
    }
    
    private IEnumerator ChangeAudianceAnimationCoroutine(float animationTime, int animationHash, bool animationState)
    {
        yield return new WaitForSeconds(animationTime);
        
        audianceAnimator.SetBool(animationHash, animationState);
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
#if UNITY_EDITOR
        Debug.Log($"changed: {_audianceSprites.Length + 1} to {sprite}");
#endif
    }
}
