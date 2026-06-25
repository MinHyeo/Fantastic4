using UnityEngine;

public class PaperTower : RangeTower
{
    [Header(nameof(PaperTower))]

    [SerializeField] private PaperProjectile _projectilePrefab;

    [SerializeField] private TowerData _data;

    private float _currentFireTimer;



    protected override void Awake()
    {
        base.Awake();


    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        Attack();
    }

    private void Attack()
    {
        _currentFireTimer += Time.deltaTime;

        // 대상이 없다면?
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
    }
}