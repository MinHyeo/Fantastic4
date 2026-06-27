using System;
using UnityEngine;

[Serializable] public class TowerRangeVisualizer
{
    [SerializeField] private float _attackRange;
    [SerializeField] private GameObject _rangeCylinder;

    public TowerRangeVisualizer(float attackRange, GameObject rangeCylinder) // Todo 다른 타워와 확인후 제거 필요
    {
        _rangeCylinder = rangeCylinder;
        SetRange(attackRange);
    }

    public void SetRange(float attackRange)
    {
        _attackRange = attackRange;
        _rangeCylinder.transform.localScale = new Vector3((2.0f * _attackRange), 0.1f, (2.0f * _attackRange));
    }
    public void ShowRange()
    {
        _rangeCylinder.SetActive(true);
    }

    public void HideRange() 
    {
        _rangeCylinder.SetActive(false);
    }
    
}
