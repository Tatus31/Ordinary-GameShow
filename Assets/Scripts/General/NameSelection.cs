using System;
using TMPro;
using UnityEngine;
using System.Collections;   

public class NameSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private DialogueManager dialogueManager;
    [Space(5)] [SerializeField] private float fixedDelay = 2.5f;
    
    public static bool IsTypingName = false;
    private string _currentName;
    private const int MaxNameLength = 3;
    private Coroutine _loopCoroutine;

    private void Start()
    {
        if (!nameText)
        {
#if UNITY_EDITOR
            Debug.LogWarning("NameSelection: NameText not set");
#endif
        }

        if (!dialogueManager)
        {
#if UNITY_EDITOR
            Debug.LogWarning("NameSelection: DialogueManager not found");
#endif
        }
    }

    private void Update()
    {
        ProcessTyping();
    }

    private void ProcessTyping()
    {
        if (!IsTypingName) 
            return;

        foreach (var c in Input.inputString)
        {
            if (c == '\b' && _currentName.Length > 0)
            {
                _currentName = _currentName.Substring(0, _currentName.Length - 1);
            }
            else if (c == '\n' || c == '\r')
            {
                FinishTyping();
                return;
            }
            else if (char.IsLetter(c) && _currentName.Length < MaxNameLength)
            {
                _currentName += char.ToUpper(c);
            }
        }
        
        nameText.text = _currentName;

        if (_currentName.Length > MaxNameLength)
        {
            FinishTyping();
        }
    }
    
    public void StartTyping()
    {        
        IsTypingName = true;
        
        _currentName = "";
        nameText.text = "";
        
        DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
        DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
        DialogueBranchManager.Instance.SetBranch("duc", false);
        DialogueBranchManager.Instance.SetBranch("IsBadWord", false);
    }

    private void FinishTyping()
    {
        IsTypingName = false;
        EvaluateTheBranches();
    }

    private void EvaluateTheBranches()
    {
        if (_loopCoroutine != null)
        {
            StopCoroutine(_loopCoroutine);
            _loopCoroutine = null;
        }
        
        if (_currentName == "")
        {
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            DialogueBranchManager.Instance.SetBranch("duc", false);
            DialogueBranchManager.Instance.SetBranch("IsBadWord", false);
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", true);
            
            _loopCoroutine = StartCoroutine(PlayNextThenPreviousDialogue());
        }
        else if (_currentName.Length < MaxNameLength)
        {
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
            DialogueBranchManager.Instance.SetBranch("duc", false);
            DialogueBranchManager.Instance.SetBranch("IsBadWord", false);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", true);
            
            _loopCoroutine = StartCoroutine(PlayNextThenPreviousDialogue());
        }
        else if (_currentName == "DUC" || _currentName == "DUK" || _currentName == "DAK" || _currentName == "DCK")
        {
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            DialogueBranchManager.Instance.SetBranch("IsBadWord", false);
            DialogueBranchManager.Instance.SetBranch("duc", true);

            StartCoroutine(WaitAndStartNextDialogue());
        }
        else if (_currentName == "DIK" || _currentName == "DIC" || _currentName == "FUC" || _currentName == "FCK" || _currentName == "PIS" || _currentName == "PUS")
        {
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            DialogueBranchManager.Instance.SetBranch("duc", false);
            DialogueBranchManager.Instance.SetBranch("IsBadWord", true);

            StartCoroutine(WaitAndStartNextDialogue());
        }
        else
        {
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            DialogueBranchManager.Instance.SetBranch("IsBadWord", false);
            DialogueBranchManager.Instance.SetBranch("nameIsValid", true);

            StartCoroutine(WaitAndStartNextDialogue());
        }
    }

    private IEnumerator WaitAndStartNextDialogue()
    {
        while (dialogueManager.IsCameraBlending)
        {
            yield return null;
        }

        yield return null;
        
        dialogueManager.StartNextDialogue();
    }

    private IEnumerator PlayNextThenPreviousDialogue()
    {
        dialogueManager.StartNextDialogue();

        while (dialogueManager.IsCameraBlending)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(fixedDelay);
        
        dialogueManager.PreviousDialogue();
        
        while (dialogueManager.IsCameraBlending)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(fixedDelay);
        
        StartTyping();
    }
}