using System;
using System.Collections;
using UnityEngine;

public class EggHitsGround : MonoBehaviour
{
    public static Action<int> OnEggDestroyed;
    
    [SerializeField] private EggSpawner eggSpawner;
    [SerializeField] private GameObject eggSplatObj;
    
    private int _eggsDestroyed = 0;

    private void Start()
    {
        _eggsDestroyed = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg"))
        {
            var eggPoints = other.gameObject.GetComponent<Egg>().EggPoints;
            
            PointManager.Instance.RemovePoints(eggPoints);
            PointManager.Instance.ChangePointsText();
            
            eggSpawner.RemoveEggFromList(other.gameObject);
            var egg = Instantiate(eggSplatObj, other.transform.position, Quaternion.identity);
            StartCoroutine(SpawnEggSplat(egg));
            AudioManager.PlaySoundWithRandomPitch("EggSplat");
            Destroy(other.gameObject);

            _eggsDestroyed++;
            OnEggDestroyed?.Invoke(_eggsDestroyed);
        }
    }
    
    private IEnumerator SpawnEggSplat(GameObject egg)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(egg);
    }
}
