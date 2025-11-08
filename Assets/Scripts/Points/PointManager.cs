using System;
using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private int currentPoints;
    [SerializeField] private  int maxPoints;
    [SerializeField] private int minPoints;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI pointsText;

    public int CurrentPoints 
    {
        get { return currentPoints; }
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
        
        AddPoints(currentPoints);
    }

    public void AddPoints(int points)
    {
        currentPoints += points;
        ChangePointsText(points);
    }
    
    public void RemovePoints(int points)
    {
        currentPoints -= points;
        ChangePointsText(points);
    }
    
    public void ChangePointsText(int points)
    {
        pointsText.text = points.ToString();
    }

    public void EvaluatePointState(int points)
    {
        if (points >= maxPoints)
        {
#if UNITY_EDITOR
            Debug.Log("Max points reached");
#endif
        }
        else if (points < minPoints)
        {
#if UNITY_EDITOR
            Debug.Log("Min points reached");
#endif
        }
    }
}
