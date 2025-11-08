using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour
{
    private Vector3 _direction;
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float baseDelay = 1f;
    
    private float _currentSpeed;
    private float _currentDelay;
    
    public void Init(Vector3 dir)
    {
        _direction = dir.normalized;
        
        _currentSpeed = baseSpeed;
        _currentDelay = baseDelay;
        
        StartCoroutine(MoveEggAfterDelay());
    }

    public void IncreaseEggSpeed(float speedIncrease = 0.5f, float delayDecrease = 0.1f)
    {
        _currentSpeed +=  speedIncrease;
        _currentDelay += Mathf.Max(0, _currentDelay - delayDecrease);
    }

    private IEnumerator MoveEggAfterDelay()
    {
        yield return new WaitForSeconds(_currentDelay);

        while (this)
        {
            transform.transform.position  += _direction * (_currentSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
