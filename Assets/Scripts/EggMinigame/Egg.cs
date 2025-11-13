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
    [SerializeField] private float fallAcceleration = 9.8f; 
    [Header("Spawn Settings")]
    [Range(0f, 1f)] [SerializeField] float spawnChance = 1f;
    [Header("Points")]
    [SerializeField] private int eggPoints = 5;
    
    private float _currentSpeed;
    private float _currentDelay;
    private float _fallSpeed;
    private bool _isFalling;

    private Coroutine _moveRoutine;
    
    private static float _globalSpeedBonus = 0f;
    private static float _globalDelayReduction = 0f;
    
    public float SpawnChance { get { return spawnChance; } set { spawnChance = value; } }
    public int  EggPoints { get { return eggPoints; } set { eggPoints = value; } }

    public void Init(Vector3 dir)
    {
        _direction = dir.normalized;

        _currentSpeed = baseSpeed + _globalSpeedBonus;
        _currentDelay = Mathf.Max(0, baseDelay - _globalDelayReduction);
        
        _fallSpeed = 0f;
        _isFalling = false;

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        _moveRoutine = StartCoroutine(MoveEggAfterDelay());
    }

    public static void IncreaseGlobalEggSpeed(float maxSpeed, float speedIncrease = 0.3f, float delayDecrease = 0.1f)
    {
        _globalSpeedBonus += speedIncrease;
        _globalSpeedBonus = Mathf.Min(_globalSpeedBonus, maxSpeed);
        
        _globalDelayReduction += delayDecrease;
    }

    private IEnumerator MoveEggAfterDelay()
    {
        yield return new WaitForSeconds(_currentDelay);

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
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FallZone"))
        {
            _isFalling = true;
        }
    }
}
