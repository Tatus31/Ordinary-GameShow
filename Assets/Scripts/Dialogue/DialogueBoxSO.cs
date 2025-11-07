using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Create new Dialogue", order = 1)]
public class DialogueBoxSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogueText;
}
