using System;
using UnityEngine;

public class EggCollection : MonoBehaviour
{
    [SerializeField] private EggSpawner eggSpawner;
    [SerializeField] private int eggPoints = 5;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg"))
        {
            PointManager.Instance.AddPoints(eggPoints);
            eggSpawner.RemoveEggFromList(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
