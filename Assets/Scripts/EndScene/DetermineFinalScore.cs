using System;
using TMPro;
using UnityEngine;

public class DetermineFinalScore : MonoBehaviour
{
    public static  DetermineFinalScore Instance;
    
    [Header("Score")]
    [SerializeField] private int minScoreF;
    [SerializeField] private int minScoreD;
    [SerializeField] private int minScoreC;
    [SerializeField] private int minScoreB;
    [SerializeField] private int minScoreA;
    
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Animator scoreBoardAnimator;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EvaluateFinalScore()
    {
        int points = PointManager.Instance.CurrentAllMinigamePoints;
        string grade;
        string branchToPlay;
        
        if (points >= minScoreA)
        {
            grade = "A";
            branchToPlay = grade;
        }
        else if (points >= minScoreB)
        {
            grade = "B";
            branchToPlay = grade;
        }
        else if (points >= minScoreC)
        {
            grade = "C";
            branchToPlay = grade;
        }
        else if (points >= minScoreD)
        {
            grade = "D";
            branchToPlay = grade;
        }
        else
        {
            grade = "F";
            branchToPlay = grade;
        }
        
        pointsText.text = $"{grade}";
        DialogueBranchManager.Instance.SetBranch(branchToPlay, true);
    }

    public void RollTheScoreBoardDown()
    {
        scoreBoardAnimator.SetTrigger("RollTheScoreBoardDown");
    }

    public void PlayYaySound()
    {
        AudioManager.PlaySound("YaySound");
    }
    
    public string GetFinalGrade()
    {
        int points = PointManager.Instance.CurrentAllMinigamePoints;
    
        if (points >= minScoreA) return "A";
        if (points >= minScoreB) return "B";
        if (points >= minScoreC) return "C";
        if (points >= minScoreD) return "D";
        return "F";
    }

    public void LoadWinOrLooseSceneBasedOnScore()
    {
        string grade =  GetFinalGrade();

        if (grade == "A" || grade == "B" || grade == "C")
        {
            SceneController.Instance.LoadSceneWithPrewarm("WinScene");
        }
        else
        {
            SceneController.Instance.LoadSceneWithPrewarm("LoseScene");
        }
    }

}
