using System;
using UnityEngine;

public class CursorState : MonoBehaviour
{
    private void Start()
    {
        LockCursor();
    }

    public static void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public static void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
