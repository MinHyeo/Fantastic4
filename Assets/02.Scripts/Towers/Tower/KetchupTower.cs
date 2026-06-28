using DG.Tweening;
using UnityEngine;

public class KetchupTower : RangeTower
{
    [Header(nameof(KetchupTower))]

    [SerializeField] private KetchupProjectile _projectilePrefab;
    [SerializeField] private Transform _ketchupHead;



    private float _currentFireTimer;



    protected override void Awake()
    {
        base.Awake();

        
    }

    protected override void Start()
    {
        base.Start();

        // _data = GameDataManager.Instance.GetData<TowerData>(_data.Id);
    }

    protected override void Update()
    {
        base.Update();

        // Attack();
        Rotate();
    }

    private void Attack()
    {
        _currentFireTimer += Time.deltaTime;

        // 가장 가까운 대상이 없다면?
        Transform enemy = _detector.FindClosestEnemy();
        if (!enemy)
        {
            return;
        }

        // 투사체가 없다면?
        if (!_projectilePrefab)
        {
            return;
        }

        // 아직 발사 쿨타임이라면?
        if (_currentFireTimer < _data.AttackSpeed)
        {
            return;
        }

        // 발사
        var projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        projectile.Initialize(0f, enemy, _data.ProjectileSpeed);
        _currentFireTimer = 0f;
        _ketchupHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }

    private void Rotate()
    {
        // 가장 가까운 대상이 없다면?
        Transform enemy = _detector.FindClosestEnemy();
        if (!enemy)
        {
            return;
        }

        // 가까운 대상을 향해 회전
        _rotator.SetLookAt(enemy);
        _rotator.Rotate(Time.deltaTime);
    }
}