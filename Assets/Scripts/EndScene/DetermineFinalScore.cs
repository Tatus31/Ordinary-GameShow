using TMPro;
using UnityEngine;

public class DetermineFinalScore : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int minScoreF;
    [SerializeField] private int minScoreD;
    [SerializeField] private int minScoreC;
    [SerializeField] private int minScoreB;
    [SerializeField] private int minScoreA;
    
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Animator scoreBoardAnimator;

    public void EvaluateFinalScore()
    {
        int points = PointManager.Instance.CurrentAllMinigamePoints;
        string grade;
        
        if (points >= minScoreA)
        {
            grade = "A";
        }
        else if (points >= minScoreB)
        {
            grade = "B";
        }
        else if (points >= minScoreC)
        {
            grade = "C";
        }
        else if (points >= minScoreD)
        {
            grade = "D";
        }
        else
        {
            grade = "F";
        }
        
        pointsText.text = $"{grade}";
    }

    public void RollTheScoreBoardDown()
    {
        scoreBoardAnimator.SetTrigger("RollTheScoreBoardDown");
    }
}
