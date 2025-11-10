using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetSpawnManager : MonoBehaviour
{
    [SerializeField] private List<TargetMover> targets = new List<TargetMover>();
    
    private readonly int _maxTargets = 10;

    private void Start()
    {
        StartCoroutine(ActivateRandomTarget(Random.Range(1,2)));
    }

    private IEnumerator ActivateRandomTarget(float time)
    {
        int targetCount = 0;
        
        while (true)
        {
            if (targets.Count > 0)
            {
                if (targetCount > _maxTargets)
                {
                    targets[0].SpeedUpTarget();
                    
                    targetCount = 0;
                    Debug.Log("Speed up the targets");
                }
                
                int randomIndex = Random.Range(0,  targets.Count);
                targets[randomIndex].StartMoving();

                targetCount++;
            } 
            
            yield return new WaitForSeconds(time);
        }
    }
}
