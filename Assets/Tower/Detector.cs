using System;
using UnityEngine;

[Serializable]
public class Detector
{
    [SerializeField] protected Transform _ownerTransform;
    [SerializeField] protected float _detectionRange;
    [SerializeField] protected LayerMask _enemyLayer;

    public Detector(Transform ownerTransform, float detectionRange, LayerMask enemyLayer)
    {
        _ownerTransform = ownerTransform;
        _detectionRange = detectionRange;
        _enemyLayer = enemyLayer;
    }

    public Collider[] FindEnemiesInRange()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(_ownerTransform.position, _detectionRange, _enemyLayer);
        return enemyColliders;
    }

    public Transform FindClosestEnemy()
    {
        Collider[] enemyColliders = FindEnemiesInRange();

        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        for(int i = 0; i < enemyColliders.Length; i++)
        {
            Transform enemyTransform = enemyColliders[i].transform;
            float distanceToEnemy = Vector3.Distance(_ownerTransform.position, enemyTransform.position);

            if(distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemyTransform;
            }
        }

        return closestEnemy;
    }

}
