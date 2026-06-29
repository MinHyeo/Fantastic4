using DG.Tweening;
using UnityEngine;

public class CheeseTower : TowerBase
{
    [Header(nameof(CheeseTower))]

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _cheeseHead;

    private float _damage;
    private float _projectileSpeed;
    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;

    public override void Init(string towerId)
    {
        base.Init(towerId);

        _data = GameDataManager.Instance.GetData<TowerData>(towerId);

        if (_data != null)
        {
            _damage = _data.AttackDamage;
            _projectileSpeed = _data.ProjectileSpeed;

            if (_data.AttackSpeed > 0)
            {
                _fireCoolTime = (1f / _data.AttackSpeed);
            }

            if (!string.IsNullOrEmpty(_data.ProjectilePath))
            {
                ResourceManager.Instance.LoadAsset<GameObject>(_data.ProjectilePath, OnProjectileLoaded);
            }
        }
        else
        {
            Debug.LogError($"[{nameof(CheeseTower)} 에러] GameDataManager에서 ID [{towerId}]에 해당하는 TowerData를 찾지 못했습니다.");
        }
    }

    protected override void Start() 
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        Rotate();

        Transform targetTransform = _detector.FindClosestEnemy();

        if (targetTransform == null)
        {
            return;
        }

        if (Time.time - _lastFireTime < _fireCoolTime)
        {
            return;
        }

        if (_projectilePrefab == null)
        {
            return;
        }

        _lastFireTime = Time.time;

        // 공격
        GameObject spawnedProjectile = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
        Projectile projectile = spawnedProjectile.GetComponent<Projectile>();
        projectile.Init(_damage, targetTransform, _projectileSpeed);

        // 공격 시 애님
        _cheeseHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }

    private void OnProjectileLoaded(GameObject loadedPrefab)
    {
        _projectilePrefab = loadedPrefab;
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
