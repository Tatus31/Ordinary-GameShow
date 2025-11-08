using UnityEngine;
using System.Collections.Generic;

public class DialogueBranchManager : MonoBehaviour
{
    public static DialogueBranchManager Instance;

    private Dictionary<string, bool> _branches = new();
    
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetBranch(string branchKey, bool expectedToBranchValue)
    {
        _branches[branchKey] = expectedToBranchValue;
    }

    public bool GetBranch(string branchKey)
    {
        return _branches.TryGetValue(branchKey, out bool value) && value;
    }
}
