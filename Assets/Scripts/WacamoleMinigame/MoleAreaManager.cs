using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoleAreaManager : MonoBehaviour
{
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

    private static float GlobalMinActivationTimeBonus;
    private static float GlobalMaxActivationTimeBonus;

    private void Start()
    {
        foreach (var moleArea in moleAreas)
        {
            moleArea.IsActive = false;
        }
                
        _currentMinActivationTime = Mathf.Max(minActivationTime, lowActivationTime - GlobalMinActivationTimeBonus);
        _currentMaxActivationTime = Mathf.Max(maxActivationTime, highActivationTime - GlobalMaxActivationTimeBonus);
        
        StartCoroutine(ActivateRandomMoleAreas(Random.Range(_currentMinActivationTime, _currentMaxActivationTime)));
    }

    private IEnumerator ActivateRandomMoleAreas(float seconds)
    {
        int molesSpawned = 0;
        
        while (true)
        {
            int randomIndex = 0;

            if (molesSpawned > maxMolesSpawned)
            {
                SpeedUpMoles();
                molesSpawned = 0;
            }
            
            if (moleAreas.Count > 0)
            {
                randomIndex = Random.Range(0,  moleAreas.Count);
                moleAreas[randomIndex].IsActive = true;
                moleAreas[randomIndex].DuckSprite.sprite = moleAreas[randomIndex].NormalDuckSprite;
                moleAreas[randomIndex].WasHit = false;
                
                molesSpawned++;
            } 
            
            yield return new WaitForSeconds(seconds);
            
            if (!moleAreas[randomIndex].WasHit)
            {
                PointManager.Instance.AddPoints(-points);
                XActivation.Instance.ActivateX();
            }
            
            moleAreas[randomIndex].IsActive = false;
        }
    }
    
    private void SpeedUpMoles()
    {
        GlobalMinActivationTimeBonus += activationTimeDecrease;
        GlobalMaxActivationTimeBonus += activationTimeDecrease;
        
        _currentMinActivationTime = Mathf.Max(minActivationTime, lowActivationTime - GlobalMinActivationTimeBonus);
        _currentMaxActivationTime = Mathf.Max(maxActivationTime, highActivationTime - GlobalMaxActivationTimeBonus);
        
        Debug.Log("Next target durations => Move: " + _currentMinActivationTime + ", Scale: " + _currentMaxActivationTime);
        Debug.Log($"Next target globalMoveDurationBonus => {GlobalMinActivationTimeBonus}");
    }
}
