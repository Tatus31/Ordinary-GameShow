using System;
using TMPro;
using UnityEngine;
using System.Collections;   

public class NameSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private DialogueManager dialogueManager;
    [Space(5)] [SerializeField] private float fixedDelay = 0.2f;
    
    public static bool IsTypingName = false;
    private string _currentName;
    private const int MaxNameLength = 3;

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
        _currentName = "";
        nameText.text = "";
        IsTypingName = true;
        
        DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
        DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
    }

    private void FinishTyping()
    {
        IsTypingName = false;
        EvaluateTheBranches();
    }

    private void EvaluateTheBranches()
    {
        if (_currentName == "")
        {
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", true);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            
            StartCoroutine(PlayNextThenPreviousDialogue());
        }
        else if (_currentName.Length < MaxNameLength)
        {
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", true);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            
            StartCoroutine(PlayNextThenPreviousDialogue());
        }
        else if (_currentName == "DUC" || _currentName == "DUK" || _currentName == "DAK" || _currentName == "DCK")
        {
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            DialogueBranchManager.Instance.SetBranch("duc", true);
            
            dialogueManager.StartNextDialogue();
        }
        else if (_currentName == "DIK" || _currentName == "DIC" || _currentName == "FUC" || _currentName == "FCK" || _currentName == "PIS" || _currentName == "PUS")
        {
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            DialogueBranchManager.Instance.SetBranch("duc", false);
            DialogueBranchManager.Instance.SetBranch("IsBadWord", true);
            
            dialogueManager.StartNextDialogue()
        }
        else
        {
            DialogueBranchManager.Instance.SetBranch("nameSpaceisEmpty", false);
            DialogueBranchManager.Instance.SetBranch("IsSmallerThanMaxValue", false);
            DialogueBranchManager.Instance.SetBranch("nameIsValid", true);
            
            dialogueManager.StartNextDialogue();
        }
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
