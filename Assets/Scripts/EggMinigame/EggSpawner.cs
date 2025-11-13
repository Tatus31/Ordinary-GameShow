using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggSpawner : MonoBehaviour
{
    public static EggSpawner Instance;

    [Serializable]
    public class EggSpawnerPosition
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 direction;
    }

    [Header("Spawner Settings")]
    [SerializeField] private EggSpawnerPosition[] positions;
    [SerializeField] private GameObject[] eggPrefabs;
    [SerializeField] private float spawnTimeCooldown = 1f;
    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 3f;
    [SerializeField] private int maxActiveEggs = 10;
    [SerializeField] private float maxSpeed = 20f;

    private float _spawnTimer;
    private int _eggPoints;
    private List<GameObject> _activeEggs = new List<GameObject>();
    private bool _isSpawning = true;
    private Coroutine _spawnRoutine;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _spawnTimer = spawnTimeCooldown;
        _isSpawning = true;
        
        _spawnRoutine = StartCoroutine(SpawnEggs());
    }

    private void Update()
    {
        if (_spawnTimer > 0)
            _spawnTimer -= Time.deltaTime;
        else
            _spawnTimer = spawnTimeCooldown;
    }

    private IEnumerator SpawnEggs()
    {
        while (_isSpawning)
        {
            _activeEggs.RemoveAll(egg => !egg);

            if (_activeEggs.Count < maxActiveEggs)
            {
                SpawnWeightedEgg();
                _eggPoints++;
            }
            else
            {
                foreach (var egg in _activeEggs)
                {
                    EggCollectionManager.Instance.SpawnEggPuff(egg);
                }


                _activeEggs.RemoveAll(egg => !egg);
            }

            if (_eggPoints >= maxActiveEggs)
            {
                float speedIncrease = 0.3f;
                maxSpawnDelay -= speedIncrease;
                maxSpawnDelay = Mathf.Max(0.5f, maxSpawnDelay - speedIncrease);
                
                foreach (var eggPrefab in eggPrefabs)
                    Egg.IncreaseGlobalEggSpeed(maxSpeed);

                _eggPoints = 0;
            }

            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnWeightedEgg()
    {
        if (positions == null || eggPrefabs == null || eggPrefabs.Length == 0)
            return;

        EggSpawnerPosition position = positions[Random.Range(0, positions.Length)];

        GameObject eggPrefab = GetWeightedEggPrefab();
        
        if (!eggPrefab)
            return;

        GameObject spawnedEgg = Instantiate(eggPrefab, position.position, position.rotation);
        _activeEggs.Add(spawnedEgg);

        Egg egg = spawnedEgg.GetComponent<Egg>();
        
        if (egg)
            egg.Init(position.direction);
    }

    private GameObject GetWeightedEggPrefab()
    {
        float totalChance = 0f;
        foreach (var eggObj in eggPrefabs)
        {
            Egg egg = eggObj.GetComponent<Egg>();
            
            if (egg)
                totalChance += egg.SpawnChance;
        }

        float randomValue = Random.value * totalChance;
        float eggSpawnChance = 0f;

        foreach (var eggObj in eggPrefabs)
        {
            Egg egg = eggObj.GetComponent<Egg>();
            
            if (!egg)
                continue;

            eggSpawnChance += egg.SpawnChance;
            
            if (randomValue <= eggSpawnChance)
                return eggObj;
        }

        return eggPrefabs[eggPrefabs.Length - 1];
    }

    public void RemoveEggFromList(GameObject egg)
    {
        _activeEggs.Remove(egg);
    }

    public void DestroyAllEggs()
    {
        _isSpawning = false;
        
        foreach (var egg in _activeEggs)
        {
            Destroy(egg);
        }
        
        _activeEggs.RemoveAll(egg => !egg);
    }

    private void OnDisable()
    {
        _isSpawning = false;
        
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (positions == null)
            return;

        Gizmos.color = Color.yellow;

        foreach (var pos in positions)
        {
            Gizmos.DrawSphere(pos.position, 0.2f);

            Vector3 forward = pos.rotation * Vector3.forward;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(pos.position, forward * 1f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(pos.position, pos.direction.normalized * 1.5f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(pos.position + Vector3.up * 0.3f, "Egg Spawn");
#endif

            Gizmos.color = Color.yellow;
        }
    }
#endif
}
