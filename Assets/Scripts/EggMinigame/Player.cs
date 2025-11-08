using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform handPivot;
    [SerializeField] private float handLowRotation = 20f;
    [SerializeField] private float handHighRotation = -30f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float xMin = -5.5f;
    [SerializeField] private float xMax = 5.1f;

    private float _rotation = 0f;
    
    private void Start()
    {
        if (!handPivot)
        {
#if UNITY_EDITOR
            Debug.LogWarning("No hand pivot found");
#endif
        }
    }

    private void Update()
    {
        float move = 0f;
        
        if (Input.GetKey(KeyCode.A))
        {
            _rotation = 0f;
            move = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _rotation = 180f;
            move = 1f;
        }

        transform.position += new Vector3(move * moveSpeed * Time.deltaTime, 0, 0);
        transform.rotation = Quaternion.Euler(0, _rotation, 0);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        transform.position = pos;
        
        float handRot = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            handRot = handHighRotation;
        }

        if (Input.GetKey(KeyCode.S))
        {
            handRot = handLowRotation;
        }
        
        handPivot.localRotation = Quaternion.Euler(0, 0, handRot);
    }
}