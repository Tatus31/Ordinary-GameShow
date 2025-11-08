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
    
    private static float _globalSpeedBonus = 0f;
    private static float _globalDelayReduction = 0f;
    private static float _globalFallTimeReduction = 0f;

    public void Init(Vector3 dir)
    {
        _direction = dir.normalized;

        _currentSpeed = baseSpeed + _globalSpeedBonus;
        _currentDelay = Mathf.Max(0, baseDelay - _globalDelayReduction);
        _currentTimeBeforeFall = Mathf.Max(0, timeBeforeFall - _globalFallTimeReduction);
        
        _fallSpeed = 0f;
        _isFalling = false;

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        _moveRoutine = StartCoroutine(MoveEggAfterDelay());
    }

    public static void IncreaseGlobalEggSpeed(float speedIncrease = 0.3f, float delayDecrease = 0.1f, float fallTimeDecrease = 0.3f)
    {
        _globalSpeedBonus += speedIncrease;
        _globalDelayReduction += delayDecrease;
        _globalFallTimeReduction += fallTimeDecrease;
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
        yield return new WaitForSeconds(_currentTimeBeforeFall);
        _isFalling = true;
    }
}
