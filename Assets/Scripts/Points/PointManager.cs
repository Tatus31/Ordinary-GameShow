using System;
using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;
    
    [SerializeField] private PointsDataSO currentPointsData;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private DialogueManager dialogueManager;
    
    [SerializeField] private int _minPoints;
    [SerializeField] private int _maxPoints;

    private bool _isScorePassable;
    private bool _isScoreInTheMiddle;
    
    public bool IsScorePassable
    {
        get { return _isScorePassable; }
    }

    public bool IsScoreInTheMiddle
    {
        get { return _isScoreInTheMiddle; }
    }
    
    public int CurrentPoints 
    {
        get { return currentPointsData.CurrentPoints; }
    }

    public int CurrentAllMinigamePoints
    {
        get { return currentPointsData.CurrentAllMinigamePoints; }
        set { currentPointsData.CurrentAllMinigamePoints = value; }
    }

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (!pointsText)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Points text not found");
#endif
            return;
        }
        
        UpdateMinMaxPoints(SceneController.LastScene);
        ChangePointsText();
    }
    
    private void UpdateMinMaxPoints(LastScene lastScene)
    {
        switch (lastScene)
        {
            case LastScene.EggMinigameScene:
                _minPoints = currentPointsData.MinPointsEgg;
                _maxPoints = currentPointsData.MaxPointsEgg;
                break;
            case LastScene.AxeThrowingMinigameScene:
                _minPoints = currentPointsData.MinPointsAxe;
                _maxPoints = currentPointsData.MaxPointsAxe;
                break;
            case LastScene.WacamoleMInigameScene:
                _minPoints = currentPointsData.MinPointsWacamole;
                _maxPoints = currentPointsData.MaxPointsWacamole;
                break;
            case LastScene.QuizMinigameScene:
                _minPoints = currentPointsData.MinPointsQuiz;
                _maxPoints = currentPointsData.MaxPointsQuiz;
                break;
            case LastScene.WhereIsWaldoMinigame:
                _minPoints = currentPointsData.MinPointsWaldo;
                _maxPoints = currentPointsData.MaxPointsWaldo;
                break;
            default:
                _minPoints = 0;
                _maxPoints = 100;
                break;
        }

#if UNITY_EDITOR
        Debug.Log($"MinPoints: {_minPoints}, MaxPoints: {_maxPoints} for last scene: {lastScene}");
#endif
    }
    
    public void AddPoints(int points)
    {
        currentPointsData.CurrentPoints += points;
        ChangePointsText();
    }
    
    public void RemovePoints(int points)
    {
        currentPointsData.CurrentPoints -= points;
        ChangePointsText();
    }
    
    public void ChangePointsText()
    {
        pointsText.text = currentPointsData.CurrentPoints.ToString();
    }

    public void EvaluatePointState()
    {
        bool reachedMax = currentPointsData.CurrentPoints >= _maxPoints;
        bool belowMin = currentPointsData.CurrentPoints < _minPoints;
        bool inMiddle = currentPointsData.CurrentPoints >= _minPoints && currentPointsData.CurrentPoints < _maxPoints;
        
        _isScorePassable = reachedMax;
        _isScoreInTheMiddle =  inMiddle;

        currentPointsData.CurrentAllMinigamePoints += currentPointsData.CurrentPoints;
        currentPointsData.CurrentPoints = 0;

        DialogueBranchManager.Instance.SetBranch("PointsPositive", reachedMax);
        DialogueBranchManager.Instance.SetBranch("PointsNeutral", inMiddle);
        DialogueBranchManager.Instance.SetBranch("PointsNegative", belowMin);

#if UNITY_EDITOR
        Debug.Log($"Passable: {_isScorePassable} Points: {currentPointsData.CurrentPoints} AllPoints: {currentPointsData.CurrentAllMinigamePoints}");
#endif
        
        if (dialogueManager)
            dialogueManager.StartNextDialogue();
        else
#if UNITY_EDITOR
            Debug.LogWarning("DialogueManager reference missing in PointManager!");
#endif
    }
}
