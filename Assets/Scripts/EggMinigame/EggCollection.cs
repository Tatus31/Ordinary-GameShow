using System;
using System.Collections;
using UnityEngine;

public class EggCollection : MonoBehaviour
{
    [SerializeField] private EggSpawner eggSpawner;
    [SerializeField] private GameObject PoofPrefab;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg"))
        {
            var eggPoints = other.gameObject.GetComponent<Egg>().EggPoints;
            
            PointManager.Instance.AddPoints(eggPoints);
            PointManager.Instance.ChangePointsText();
            
            eggSpawner.RemoveEggFromList(other.gameObject);
            var egg = Instantiate(PoofPrefab, other.transform.position, Quaternion.identity);
            EggCollectionManager.Instance.SpawnEggPuff(egg);
            Destroy(other.gameObject);
        }
    }
}
