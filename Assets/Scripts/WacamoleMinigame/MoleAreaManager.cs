using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoleAreaManager : MonoBehaviour
{
    public static event Action<int> OnDuckEscaped;
    
    [SerializeField] private List<MoleArea> moleAreas = new List<MoleArea>();
    [Header("Activation Time")]
    [SerializeField] private float lowActivationTime = 3f;
    [SerializeField] private float highActivationTime = 5f;
    [SerializeField] private float minActivationTime = 0.75f;
    [SerializeField] private float maxActivationTime = 1f;
    [Header("Activation Duration")]
    [SerializeField] private float activationTimeDecrease = 0.1f;
    [SerializeField] private float maxMolesSpawned = 10f;
    [Header("Points")]
    [SerializeField] private int points;
    
    private float _currentMinActivationTime;
    private float _currentMaxActivationTime;

    private int _escapedDucks = 0;
    
    private bool _isSpawning = true;
    private bool _skipFirstMole = true;

    private static float GlobalMinActivationTimeBonus;
    private static float GlobalMaxActivationTimeBonus;

    private void Start()
    {
        foreach (var moleArea in moleAreas)
        {
            moleArea.IsActive = false;
        }

        _escapedDucks = 0;
        _isSpawning = true;
                
        _currentMinActivationTime = Mathf.Max(minActivationTime, lowActivationTime - GlobalMinActivationTimeBonus);
        _currentMaxActivationTime = Mathf.Max(maxActivationTime, highActivationTime - GlobalMaxActivationTimeBonus);
        
        StartCoroutine(ActivateRandomMoleAreas(Random.Range(_currentMinActivationTime, _currentMaxActivationTime)));
    }

    private IEnumerator ActivateRandomMoleAreas(float seconds)
    {
        int molesSpawned = 0;

        while (_isSpawning)
        {
            if (molesSpawned > maxMolesSpawned)
            {
                SpeedUpMoles();
                molesSpawned = 0;
            }

            int molesToSpawn = Random.Range(1, Mathf.Min(4, moleAreas.Count + 1));
            List<int> spawnedIndices = new List<int>();

            for (int i = 0; i < molesToSpawn; i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, moleAreas.Count);
                } while (spawnedIndices.Contains(randomIndex));

                spawnedIndices.Add(randomIndex);

                MoleArea mole = moleAreas[randomIndex];
                mole.IsActive = true;
                mole.DuckSprite.sprite = mole.NormalDuckSprite;
                mole.WasHit = false;

                molesSpawned++;
                
                yield return new WaitUntil(() => mole.CurrentState == MoleState.Visible);
            }
            
            yield return new WaitForSeconds(seconds);

            foreach (int index in spawnedIndices)
            {
                MoleArea mole = moleAreas[index];
                
                if (!mole.WasHit)
                {
                    mole.IsActive = false;

                    yield return new WaitUntil(() => mole.CurrentState == MoleState.Hidden);

                    if (_skipFirstMole)
                    {
                        _skipFirstMole = false;
                        continue;
                    }

                    PointManager.Instance.AddPoints(-points);
                    XActivation.Instance.ActivateX();

                    _escapedDucks++;
                    OnDuckEscaped?.Invoke(_escapedDucks);
                }
                else
                {
                    mole.IsActive = false;
                }
            }

        }
    }

    public void DestroyAllDucks()
    {
        _isSpawning = false;
        
        foreach (var moleArea in moleAreas)
        {
            Destroy(moleArea.gameObject);
        }
        
        moleAreas.RemoveAll(mole => !mole);
    }
    
    private void SpeedUpMoles()
    {
        GlobalMinActivationTimeBonus += activationTimeDecrease;
        GlobalMaxActivationTimeBonus += activationTimeDecrease;
        
        _currentMinActivationTime = Mathf.Max(minActivationTime, lowActivationTime - GlobalMinActivationTimeBonus);
        _currentMaxActivationTime = Mathf.Max(maxActivationTime, highActivationTime - GlobalMaxActivationTimeBonus);
    }
}
