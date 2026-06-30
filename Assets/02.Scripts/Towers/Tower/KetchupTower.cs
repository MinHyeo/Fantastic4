using DG.Tweening;
using UnityEngine;

public class KetchupTower : TowerBase
{
    [Header(nameof(KetchupTower))]

    private GameObject _projectilePrefab;
    [SerializeField] private Transform _ketchupHead;

    private string _abilityId;

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

        _abilityId = Data.AbilityId;

        if (!string.IsNullOrEmpty(_data.ProjectilePath))
        {
            ResourceManager.Instance.LoadAsset<GameObject>(_data.ProjectilePath, OnProjectileLoaded);
        }
    }

    private void Attack()
    {
        _currentFireTimer += Time.deltaTime;

        // 가장 가까운 대상이 없다면?
        Transform enemy = _detector.FindLeadEnemy();
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
        float fireInterval = 1f / _data.AttackSpeed;
        if (_currentFireTimer < fireInterval)
        {
            return;
        }

        // 발사
        var projectileInst = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        var ketchupProjectile = projectileInst.GetComponent<KetchupProjectile>();
        ketchupProjectile.Init(0f, enemy, _data.ProjectileSpeed, _data.AbilityId);
        _currentFireTimer = 0f;

        // 발사 애님
        _ketchupHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
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