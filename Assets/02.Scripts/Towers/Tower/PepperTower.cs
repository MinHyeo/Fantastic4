using UnityEngine;

public class PepperTower : RangeTower
{
    [Header(nameof(PepperTower))]

    [SerializeField] private PepperProjectile _projectilePrefab;
    [SerializeField] private Transform _pepperHead;


    private float _currentFireTimer;



    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        // 처음 포탑이 생성될 때, 바로 사격 가능하도록
        _currentFireTimer = _data.AttackSpeed;
    }

    protected override void Update()
    {
        base.Update();

        Attack();
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