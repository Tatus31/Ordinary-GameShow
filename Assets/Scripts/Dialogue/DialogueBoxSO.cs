using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Create new Dialogue", order = 1)]
public class DialogueBoxSO : ScriptableObject
{
    public BranchingDialogue[] dialogueBoxes;
    public bool IsTheRightAnswer;
}

[Serializable]
public class BranchingDialogue
{
    [TextArea(3, 10)]
    public string DialogueText;
    public string BranchKey;
    public bool IsExpectedToBranch;

}
