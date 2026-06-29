using DG.Tweening;
using UnityEngine;

public class PepperTower : TowerBase
{
    [Header(nameof(PepperTower))]

    private GameObject _projectilePrefab;
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

        if (!string.IsNullOrEmpty(_data.ProjectilePath))
        {
            ResourceManager.Instance.LoadAsset<GameObject>(_data.ProjectilePath, OnProjectileLoaded);
        }
    }

    private void Attack()
    {
        _currentFireTimer += Time.deltaTime;

        // 가장 가까운 적을 찾습니다.
        Transform enemy = _detector.FindLeadEnemy();
        if (!enemy)
        {
            return;
        }

        // 투사체 프리팹이 없으면 발사하지 않습니다.
        if (!_projectilePrefab)
        {
            return;
        }

        // 아직 발사 쿨타임이 끝나지 않았으면 발사하지 않습니다.
        float fireInterval = 1f / _data.AttackSpeed;
        if (_currentFireTimer < fireInterval)
        {
            return;
        }

        // 발사
        var projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        var pepperProjectile = projectile.GetComponent<PepperProjectile>();
        pepperProjectile.Init(_data.AttackDamage, enemy, _data.ProjectileSpeed);
        _currentFireTimer = 0;

        // 발사 애님
        _pepperHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }

    private void Rotate()
    {
        // 최선두 적이 있는지?
        Transform enemy = _detector.FindLeadEnemy();
        if (!enemy)
        {
            return;
        }

        // 최선두 대상을 향해 회전
        _rotator.SetLookAt(enemy);
        _rotator.Rotate(Time.deltaTime);
    }

    private void OnProjectileLoaded(GameObject loadedPrefab)
    {
        _projectilePrefab = loadedPrefab;
    }
}