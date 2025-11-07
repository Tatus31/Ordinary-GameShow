using System;
using TMPro;
using UnityEngine;

public class NameSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    private void Start()
    {
        if (!nameText)
        {
            Debug.LogWarning("NameSelection: NameText not set");
            return;
        }
    }

    public void ChangeNameText(string newName)
    {
        nameText.text = newName;
    }
}
