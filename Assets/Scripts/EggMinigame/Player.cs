using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // [SerializeField] private Transform handPivot;
    // [SerializeField] private float handLowRotation = 20f;
    // [SerializeField] private float handHighRotation = -30f;
    // [SerializeField] private float moveSpeed = 5f;
    // [SerializeField] private float xMin = -5.5f;
    // [SerializeField] private float xMax = 5.1f;
    
    [SerializeField] private Sprite wolfLowSprite;
    [SerializeField] private Sprite wolfHighSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private BoxCollider2D boxCollider2DHigh;
    [SerializeField] private BoxCollider2D boxCollider2DLow;

    private float _rotation = 0f;
    
    private Coroutine _playWooshSound;
    private bool _isWooshPlaying = false;
    
    private void Start()
    {
//         if (!handPivot)
//         {
// #if UNITY_EDITOR
//             Debug.LogWarning("No hand pivot found");
// #endif
//         }
    }

    private void Update()
    {
        //float move = 0f;
        
        if (Input.GetKey(KeyCode.D))
        {
            _rotation = 0f;
            TryPlayWoosh();
            //move = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _rotation = 180f;
            TryPlayWoosh();
            //move = 1f;
        }

        // transform.position += new Vector3(move * moveSpeed * Time.deltaTime, 0, 0);
        transform.rotation = Quaternion.Euler(0, _rotation, 0);

        // Vector3 pos = transform.position;
        // pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        // transform.position = pos;
        
        // float handRot = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            spriteRenderer.sprite = wolfHighSprite;
            boxCollider2DHigh.gameObject.SetActive(true);
            boxCollider2DLow.gameObject.SetActive(false);
            TryPlayWoosh();
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            spriteRenderer.sprite = wolfLowSprite;
            boxCollider2DLow.gameObject.SetActive(true);
            boxCollider2DHigh.gameObject.SetActive(false);
            TryPlayWoosh();
        }
        //
        // handPivot.localRotation = Quaternion.Euler(0, 0, handRot);
    }
    
    private void TryPlayWoosh()
    {
        if (!_isWooshPlaying)
            _playWooshSound = StartCoroutine(PlayWooshSound());
    }
    
    private IEnumerator PlayWooshSound()
    {
        _isWooshPlaying  = true;
        AudioManager.PlaySoundWithRandomPitch("EggWoosh");
        yield return new WaitForSeconds(0.45f);
        _isWooshPlaying  = false;
    }
}