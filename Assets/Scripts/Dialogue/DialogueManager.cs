using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class DialogueManager : MonoBehaviour
{
    [Serializable]
    private class DialogueBox
    {
        public DialogueBoxSO[] Dialogues;
        public CinemachineCamera Camera;
        public bool isForcedToNextDialogue;
        public Sprite Sprite;
        [Range(0, 60)]
        public float timeToNextDialogue;
        [Space(5)]
        public UnityEvent OnDialogueComplete;
    }
    
    [SerializeField] private DialogueBox[] dialogueBoxes;
    [Space(10f)]
    [Header("Script Refrences")]
    [SerializeField] private TypeWriter typeWriter;
    [Header("Object Refrences")]
    [SerializeField] private TextMeshProUGUI dialogueBoxText;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CinemachineCamera startingCamera;

    private int _currentDialogueIndex = -1;
    private bool _isDialogueChanging = false;
    
    private DialogueBox _currentDialogueBox;
    
    private Coroutine _forceNextDialogueRoutine;

    public bool IsCameraBlending => cinemachineBrain && cinemachineBrain.IsBlending;
    
    private void Awake()
    {
        foreach (var dialogueBox in dialogueBoxes)
        {
            dialogueBox.Camera.Priority = 0;
        }
        
        dialogueBoxText.text = "";
    }

    private void Start()
    {
        if (!dialogueBoxText)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Dialogue Box Text is null");
#endif
        }

        if (!typeWriter)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Dialogue Box TypeWriter is null");
#endif
        }

        if (!cinemachineBrain)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Cinemachine Brain is null");
#endif
        }

        if (!spriteRenderer)
        {
#if UNITY_EDITOR
            Debug.LogWarning("spriteRenderer is null");
#endif
        }
        
        startingCamera.Priority = 2;
        StartNextDialogue();
    }

    private void Update()
    {
        if(NameSelection.IsTypingName)
            return;
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (typeWriter.IsTyping)
            {
                typeWriter.SkipTyping(() =>
                {
                    _currentDialogueBox?.OnDialogueComplete?.Invoke();
                    _forceNextDialogueRoutine = StartCoroutine(ForceNextDialogue());
                });
            }
            else
            {
                StartNextDialogue();
            }
        }
    }

    public void StartNextDialogue()
    {
        if(_isDialogueChanging)
            return;
        
        StartCoroutine(StartNextDialogueRoutine());
    }

    public void PreviousDialogue()
    {
        if(_isDialogueChanging)
            return;
        
        StartCoroutine(PreviousDialogueRoutine());
    }

    private IEnumerator StartNextDialogueRoutine()
    {
        _isDialogueChanging = true;
        
        if (_currentDialogueIndex >= 0 && _currentDialogueIndex < dialogueBoxes.Length)
        {
            dialogueBoxes[_currentDialogueIndex].Camera.Priority = 0;
            spriteRenderer.sprite = dialogueBoxes[_currentDialogueIndex].Sprite;
        }

        _currentDialogueIndex++;

        if (_currentDialogueIndex >= dialogueBoxes.Length)
        {
            dialogueBoxText.text = "Finished all dialogue";
            _isDialogueChanging = false;
            yield break;
        }
        
        _currentDialogueBox = dialogueBoxes[_currentDialogueIndex];
        _currentDialogueBox.Camera.Priority = 10;
        spriteRenderer.sprite = dialogueBoxes[_currentDialogueIndex].Sprite;
        
        ForceDialogueRoutine();

        yield return null;
        
        _isDialogueChanging = false;
    }

    private IEnumerator PreviousDialogueRoutine()
    {
        _isDialogueChanging = true;
        
        if (_forceNextDialogueRoutine != null)
        {
            StopCoroutine(_forceNextDialogueRoutine);
            _forceNextDialogueRoutine = null;
        }

        if (_currentDialogueIndex >= 0 && _currentDialogueIndex < dialogueBoxes.Length)
        {
            dialogueBoxes[_currentDialogueIndex].Camera.Priority = 0;
            spriteRenderer.sprite = dialogueBoxes[_currentDialogueIndex].Sprite;
        }
        
        _currentDialogueIndex--;

        if (_currentDialogueIndex < 0)
        {
            _currentDialogueIndex = 0;
#if UNITY_EDITOR
            Debug.LogWarning("Dialogue Box Index already at first dialogue");
#endif
        }
        
        _currentDialogueBox = dialogueBoxes[_currentDialogueIndex];
        _currentDialogueBox.Camera.Priority = 10;
        spriteRenderer.sprite = dialogueBoxes[_currentDialogueIndex].Sprite;
        
                
        ForceDialogueRoutine();
        
        yield return null;
        
        _isDialogueChanging = false;
    }

    private void ForceDialogueRoutine()
    {
        StartCoroutine(StartTypingAfterBlend(_currentDialogueBox));
    }

    private IEnumerator ForceNextDialogue()
    {
        if(_forceNextDialogueRoutine != null)
            StopCoroutine(_forceNextDialogueRoutine);
        
        if (_currentDialogueBox != null && _currentDialogueBox.isForcedToNextDialogue)
        {
            yield return new WaitForSeconds(_currentDialogueBox.timeToNextDialogue);
            StartNextDialogue();
        }
    }

    private IEnumerator StartTypingAfterBlend(DialogueBox dialogueBox)
    {
        if (cinemachineBrain)
        {
            while (cinemachineBrain.IsBlending)
            {
                yield return null; 
            }
        }

        var dialogueBranch = GetBranchDialogue(dialogueBox.Dialogues);

        if (dialogueBranch == null)
        {
            Debug.LogWarning("Dialogue Box doesn't have a Dialogue Branch");
            yield break;
        }
        
        typeWriter.StartTyping(dialogueBranch.DialogueText, () =>
        {
            dialogueBox.OnDialogueComplete?.Invoke();
            
            if (dialogueBox.isForcedToNextDialogue)
                _forceNextDialogueRoutine = StartCoroutine(ForceNextDialogue());
        });
    }

    private BranchingDialogue GetBranchDialogue(DialogueBoxSO[] dialogues)
    {
        BranchingDialogue fallbackDialogue = null;
        
        foreach (var dialogue in dialogues)
        {
            foreach (var branch in dialogue.dialogueBoxes)
            {
                if (string.IsNullOrEmpty(branch.BranchKey))
                {
                    if (fallbackDialogue == null)
                    {
                        fallbackDialogue = branch;
                        continue;
                    }
                }
                
                bool currentBranchValue = DialogueBranchManager.Instance.GetBranch(branch.BranchKey);
                
                if (currentBranchValue == branch.IsExpectedToBranch)
                    return branch;
            }
        }
        
        return fallbackDialogue;
    }
}
