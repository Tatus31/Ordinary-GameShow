using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour
{
    private Vector3 _direction;
    private Vector3 _fallDirection = Vector3.down;

    [Header("Base Movement Settings")]
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float baseDelay = 1f;

    [Header("Falling Settings")]
    [SerializeField] private float timeBeforeFall = 2f; 
    [SerializeField] private float fallAcceleration = 9.8f; 

    private float _currentSpeed;
    private float _currentDelay;
    private float _currentTimeBeforeFall;
    
    private float _fallSpeed;
    private bool _isFalling;

    private Coroutine _moveRoutine;

    public void Init(Vector3 dir)
    {
        _direction = dir.normalized;

        _currentSpeed = baseSpeed;
        _currentDelay = baseDelay;
        _currentTimeBeforeFall = timeBeforeFall;
        
        _fallSpeed = 0f;
        _isFalling = false;

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        _moveRoutine = StartCoroutine(MoveEggAfterDelay());
    }

    public void IncreaseEggSpeed(float speedIncrease = 0.5f, float delayDecrease = 0.1f, float fallTimeDecrease = 0.2f)
    {
        _currentSpeed += speedIncrease;
        _currentDelay = Mathf.Max(0, _currentDelay - delayDecrease);
        _currentTimeBeforeFall = Mathf.Max(0, _currentTimeBeforeFall - fallTimeDecrease);
    }

    private IEnumerator MoveEggAfterDelay()
    {
        yield return new WaitForSeconds(_currentDelay);

        StartCoroutine(StartFallingAfterTime());

        while (true)
        {
            Vector3 move = _direction * (_currentSpeed * Time.deltaTime);

            if (_isFalling)
            {
                _fallSpeed += fallAcceleration * Time.deltaTime;
                move += _fallDirection * (_fallSpeed * Time.deltaTime);
            }

            transform.position += move;
            yield return null;
        }
    }

    private IEnumerator StartFallingAfterTime()
    {
        yield return new WaitForSeconds(timeBeforeFall);
        _isFalling = true;
    }
}
