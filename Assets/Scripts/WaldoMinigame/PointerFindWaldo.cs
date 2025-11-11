using System;
using Unity.VisualScripting;
using UnityEngine;

public class PointerFindWaldo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Waldo"))
        {
            Debug.Log("Waldo");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Waldo"))
        {
            //other.gameObject.GetComponent<SpriteRenderer>().sprite;
            Debug.Log("Waldo");
        }
    }
}
