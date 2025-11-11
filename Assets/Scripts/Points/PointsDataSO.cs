using UnityEngine;

[CreateAssetMenu(fileName = "New Point Box", menuName = "Points/Create new PointBox", order = 2)]
public class PointsDataSO : ScriptableObject
{
    public int CurrentPoints = 0;
    [Header("Egg minigame")]
    public int MaxPointsEgg;
    public int MinPointsEgg;
    [Header("Axe minigame")]
    public int MaxPointsAxe;
    public int MinPointsAxe;
    [Header("Wacamole minigame")]
    public int MaxPointsWacamole;
    public int MinPointsWacamole;
    [Header("Quiz minigame")]
    public int MaxPointsQuiz;
    public int MinPointsQuiz;
    [Header("Waldo minigame")]
    public int MaxPointsWaldo;
    public int MinPointsWaldo;
}
