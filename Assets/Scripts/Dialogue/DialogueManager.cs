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
        public DialogueBoxSO Dialogue;
        public CinemachineCamera Camera;
        public UnityEvent OnDialogueComplete;
    }
    
    [SerializeField] private DialogueBox[] dialogueBoxes;
    [Space(10f)]
    [SerializeField] private TextMeshPro dialogueBoxText;

    [SerializeField] private TypeWriter typeWriter;
    [SerializeField] private CinemachineBrain cinemachineBrain;

    private int _currentDialogueIndex = -1;
    
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
            Debug.LogWarning("Dialogue Box Text is null");
        }

        if (!typeWriter)
        {
            Debug.LogWarning("Dialogue Box TypeWriter is null");
        }

        if (!cinemachineBrain)
        {
            Debug.LogWarning("Cinemachine Brain is null");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (typeWriter.IsTyping)
            {
                typeWriter.SkipTyping();
            }
            else
            {
                StartDialogue();
            }
        }
    }

    private void StartDialogue()
    {
        if (_currentDialogueIndex >= 0 && _currentDialogueIndex < dialogueBoxes.Length)
        {
            dialogueBoxes[_currentDialogueIndex].Camera.Priority = 0;
        }

        _currentDialogueIndex++;

        if (_currentDialogueIndex >= dialogueBoxes.Length)
        {
            dialogueBoxText.text = "Finished all dialogue";
            return;
        }
        
        DialogueBox currentDialogueBox = dialogueBoxes[_currentDialogueIndex];
        currentDialogueBox.Camera.Priority = 10;

        StartCoroutine(StartTypingAfterBlend(currentDialogueBox));
    }

    
    //TODO: Find a fix for blending fucking up the rendering of the text 
    private IEnumerator StartTypingAfterBlend(DialogueBox dialogueBox)
    {
        if (cinemachineBrain)
        {
            while (cinemachineBrain.IsBlending)
            {
                yield return null; 
            }
            
            yield return new WaitForEndOfFrame();

            typeWriter.StartTyping(dialogueBox.Dialogue.dialogueText, () =>
            {
                dialogueBox.OnDialogueComplete?.Invoke();
                Debug.Log(dialogueBox.Dialogue.dialogueText);
            });
        }

    }
}
