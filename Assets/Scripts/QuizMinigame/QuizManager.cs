using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [Serializable]
    private class QuizBox
    {
        [Header("Question")]
        public DialogueBoxSO QuizQuestion;
        [Header("Answers")]
        public DialogueBoxSO QuizAnswerA;
        public DialogueBoxSO QuizAnswerB;
        public DialogueBoxSO QuizAnswerC;
        public DialogueBoxSO QuizAnswerD;
    }
    
    [SerializeField] private RectTransform quizCanvGroup;
    [SerializeField] private QuizBox[] quizBoxes;
    [Header("Refrences")]
    [SerializeField] private TypeWriter typeWriter; 
    [Header("Canvas References")]
    [SerializeField] private TextMeshProUGUI quizQuestion;
    [SerializeField] private TextMeshProUGUI quizAnswerA;
    [SerializeField] private TextMeshProUGUI quizAnswerB;
    [SerializeField] private TextMeshProUGUI quizAnswerC;
    [SerializeField] private TextMeshProUGUI quizAnswerD;

    public static bool IsAnsweringQuestions;
    
    private int _currentQuestionIndex = -1;
    private QuizBox  _currentQuestionBox;

    private void Start()
    {
        if(quizCanvGroup)
            quizCanvGroup.gameObject.SetActive(false);
    }

    public void StartQuiz()
    {
        IsAnsweringQuestions =  true;
        StartCoroutine(FadeAnswerRectIn());
    }

    private IEnumerator FadeAnswerRectIn()
    {
        quizCanvGroup.gameObject.SetActive(true);
        var answerCanvasGroup = quizCanvGroup.GetComponent<CanvasGroup>();

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var rectA = Mathf.Lerp(0f, 1f, elapsed / duration);
            answerCanvasGroup.alpha = rectA;
            
            yield return null; 
        }

        NextQuestion();
    }

    private void NextQuestion()
    {
        _currentQuestionIndex++;
        
        if (_currentQuestionIndex >= quizBoxes.Length)
        {
            quizQuestion.text = "Finished all questions";
            return;
        }
        
        _currentQuestionBox  = quizBoxes[_currentQuestionIndex];
        StartCoroutine(StartTypingText(_currentQuestionBox));
    }
    
    
    private IEnumerator StartTypingText(QuizBox quizBox)
    {
        typeWriter.StartTypingMultiple(new[]
            {
                quizQuestion,
                quizAnswerA,
                quizAnswerB,
                quizAnswerC,
                quizAnswerD
            }, 
            new[]
            {
                quizBox.QuizQuestion.dialogueBoxes[0].DialogueText,
                quizBox.QuizAnswerA.dialogueBoxes[0].DialogueText,
                quizBox.QuizAnswerB.dialogueBoxes[0].DialogueText,
                quizBox.QuizAnswerC.dialogueBoxes[0].DialogueText,
                quizBox.QuizAnswerD.dialogueBoxes[0].DialogueText
            });

        yield return null;
    }
}
