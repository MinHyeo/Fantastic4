using UnityEngine;

public class CheeseTower : TowerBase
{
    [Header("임시 동작 테스트용")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _damage = 10.0f;
    [SerializeField] private float _projectileSpeed = 5.0f;
    [SerializeField] private Transform _firePoint;


    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;

    protected override void Update()
    {
        base.Update();

        Transform targetTransform = _detector.FindClosestEnemy();

        if (targetTransform == null)
        {
            return;
        }

        if (Time.time - _lastFireTime < _fireCoolTime)
        {
            return;
        }


        AttackSystem.Attack(_projectilePrefab, _firePoint, targetTransform, _damage, _projectileSpeed);
        _lastFireTime = Time.time;


    }
}
