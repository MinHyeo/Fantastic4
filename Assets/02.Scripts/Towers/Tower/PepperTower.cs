using DG.Tweening;
using UnityEngine;

public class PepperTower : TowerBase
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
    }

    protected override void Update()
    {
        base.Update();

        Attack();
        Rotate();
    }

    public override void Init(string towerId)
    {
        base.Init(towerId);

        // 처음 포탑이 생성될 때, 바로 사격 가능하도록
        _currentFireTimer = _data.AttackSpeed;
    }

    private void Attack()
    {
        _currentFireTimer += Time.deltaTime;

        // 가장 가까운 적을 찾습니다.
        Transform enemy = _detector.FindClosestEnemy();
        if (!enemy)
        {
            return;
        }

        // 투사체 프리팹이 없으면 발사하지 않습니다.
        if (!_projectilePrefab)
        {
            return;
        }

        // AttackSpeed를 초당 발사 수로 사용합니다.
        float fireInterval = 1f / _data.AttackSpeed;

        // 아직 발사 쿨타임이 끝나지 않았으면 발사하지 않습니다.
        if (_currentFireTimer < fireInterval)
        {
            return;
        }

        // 투사체를 생성합니다.
        var projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        projectile.Init(_data.AttackDamage, enemy, _data.ProjectileSpeed);

        // 누적 오차를 줄이기 위해 0으로 초기화하지 않고 발사 간격만큼 뺍니다.
        _currentFireTimer = 0;

        // 발사 시 머리 애니메이션을 재생합니다.
        _pepperHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
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