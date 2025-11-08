using System;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField]private TextMeshProUGUI typingText;
    
    private bool _isTyping;
    private string _text;
    private Coroutine _typingCoroutine;
    
    public bool IsTyping 
    {
        get { return _isTyping; }
    }
    
    public string Text
    {
        get { return _text; }
    }

    private void Awake()
    {
        if(!typingText)
            typingText  = GetComponent<TextMeshProUGUI>();
    }

    public void StartTyping(string text, Action onComplete = null)
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _text = text;
        _typingCoroutine = StartCoroutine(TypingCoroutine(onComplete));
    }

    public void SkipTyping(Action onComplete)
    {
        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
            typingText.text = _text;
            _isTyping = false;
            
            onComplete?.Invoke();
        }
    }

    private IEnumerator TypingCoroutine(Action onComplete)
    {
        _isTyping = true;
        typingText.text = "";
        
        foreach (char c in _text)
        {
            typingText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        _isTyping = false;
        
        yield return new WaitUntil(() => !_isTyping);
        
        onComplete?.Invoke();
    } 
}
