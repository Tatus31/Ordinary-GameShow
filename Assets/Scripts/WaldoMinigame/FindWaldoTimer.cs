using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FindWaldoTimer : MonoBehaviour
{
    public static event Action<int> OnStartTransition;
    
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timer;
    
    private float _currentTime;
    private bool _hasStartedTransition = false;
    private bool _startTimer = false;

    public static bool HasFoundWaldo = false;

    private void Start()
    {
        HasFoundWaldo  = false;
        _currentTime = timer;
        _startTimer  = false;
        
        StartCoroutine(StartTimer(2));
    }

    private void Update()
    {
        if(!_startTimer)
            return;
        
        if (!HasFoundWaldo && !_hasStartedTransition)
        {
            _currentTime -= Time.deltaTime;
            timerText.text = _currentTime.ToString("F2");

            if (_currentTime <= 0)
            {
                _currentTime = 0;

                if (!_hasStartedTransition)
                {
                    OnStartTransition?.Invoke((int)timer);
                    _hasStartedTransition = true;
                }

                timerText.text = "00:00";
            }
        }
    }

    private IEnumerator StartTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        _startTimer = true;
    }
}
