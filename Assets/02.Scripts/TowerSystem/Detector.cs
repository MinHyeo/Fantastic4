using System;
using System.Collections.Generic;
using UnityEngine;


public class Detector : MonoBehaviour
{
    [SerializeField] protected float _detectionRange;
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] private SphereCollider _rangeCollider;

    private List<Collider> _enemiesInRange = new List<Collider>();


    public float DetectionRange
    {
        get { return _detectionRange; }
        set { 
                _detectionRange = value;
                if (_rangeCollider != null)
                {
                    _rangeCollider.radius = value;
                }
            }
    }

    private void AddEnemy(Collider enemy)
    {
        if (IsEnemyLayer(enemy.gameObject.layer) == false)
        {
            return;
        }
        _enemiesInRange.Add(enemy);
    }

    private void RemoveEnemy(Collider enemy)
    {
        _enemiesInRange.Remove(enemy);
    }

    public Collider[] FindEnemiesInRange()
    {
        // 방식 변경시 삭제필요
        //Collider[] enemyColliders = Physics.OverlapSphere(_ownerTransform.position, _detectionRange, _enemyLayer);
        //return enemyColliders;
        RemoveDeadEnemies();
        return _enemiesInRange.ToArray();
    }

    public Transform FindClosestEnemy()
    {
        Collider[] enemiesInRange = FindEnemiesInRange();
        GameObject leadEnemy = BattleManager.Instance.GetBeInTheLead(enemiesInRange);
        

        if (leadEnemy == null)
        {
            return null;
        }
        return leadEnemy.transform;
    }
    private bool IsEnemyLayer(int layer)
    {
        int layerBit = (1 << layer);
        if ((layerBit & _enemyLayer.value) == 0)
        {
            return false;
        }

        return true;
    }

    private void RemoveDeadEnemies()
    {
        for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
        {
            Collider enemy = _enemiesInRange[i];
            if (enemy == null || enemy.gameObject.activeSelf == false)
            {
                _enemiesInRange.RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AddEnemy(other);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveEnemy(other);
    }

    



}