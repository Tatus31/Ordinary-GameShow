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

    public void StartTyping(TextMeshProUGUI textArea, string text, Action onComplete = null)
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _text = text;
        _typingCoroutine = StartCoroutine(TypingCoroutine(onComplete, textArea));
    }
    
    public void StartTypingMultiple(TextMeshProUGUI[] textAreas, string[] texts, Action onComplete = null)
    {
        if (textAreas == null || texts == null || textAreas.Length != texts.Length)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Text areas missing");
#endif
            return;
        }

        StartCoroutine(TypingMultipleCoroutine(textAreas, texts, onComplete));
    }

    public void SkipTyping(Action onComplete, TextMeshProUGUI textArea)
    {
        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
            textArea.text = _text;
            _isTyping = false;
            
            onComplete?.Invoke();
        }
    }

    private IEnumerator TypingCoroutine(Action onComplete, TextMeshProUGUI textArea)
    {
        _isTyping = true;
        textArea.text = "";
        
        foreach (char c in _text)
        {
            textArea.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        _isTyping = false;
        onComplete?.Invoke();
    } 
    
    private IEnumerator TypingMultipleCoroutine(TextMeshProUGUI[] textAreas, string[] texts, Action onComplete)
    {
        _isTyping = true;

        foreach (var textArea in textAreas)
        {
            textArea.text = "";
        }

        int maxLength = 0;

        foreach (var t in texts)
        {
            maxLength = Mathf.Max(maxLength, t.Length);
        }

        for (int i = 0; i < maxLength; i++)
        {
            for (int j = 0; j < textAreas.Length; j++)
            {
                if (i < texts[j].Length)
                {
                    textAreas[j].text += texts[j][i];
                }
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false;
        onComplete?.Invoke();
    }
}
