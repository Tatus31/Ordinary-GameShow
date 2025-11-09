using System.Collections;
using UnityEngine;

public class EggHitsGround : MonoBehaviour
{
    [SerializeField] private EggSpawner eggSpawner;
    [SerializeField] private GameObject eggSplatObj;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg"))
        {
            var eggPoints = other.gameObject.GetComponent<Egg>().EggPoints;
            
            PointManager.Instance.AddPoints(eggPoints);
            PointManager.Instance.ChangePointsText();
            
            eggSpawner.RemoveEggFromList(other.gameObject);
            var egg = Instantiate(eggSplatObj, other.transform.position, Quaternion.identity);
            StartCoroutine(SpawnEggSplat(egg));
            Destroy(other.gameObject);
        }
    }
    
    private IEnumerator SpawnEggSplat(GameObject egg)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(egg);
    }
}
