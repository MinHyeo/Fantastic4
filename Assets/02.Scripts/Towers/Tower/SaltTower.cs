using DG.Tweening;
using UnityEngine;

public class SaltTower : TowerBase
{
    [Header(nameof(SaltTower))]

    private GameObject _projectilePrefab;
    [SerializeField] private Transform _saltHead;

    private SaltDebuff _saltDebuff;

    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;
    private float _damage;
    private float _projectileSpeed;

    public override void Init(string towerId)
    {
        _data = GameDataManager.Instance.GetData<TowerData>(towerId);

        if (_data != null)
        {
            _damage = _data.AttackDamage;
            _projectileSpeed = _data.ProjectileSpeed;

            if (_detector != null)
            {
                _detector.SetRange(_data.AttackRange);
            }

            if (_rangeVisualizer != null)
            {
                _rangeVisualizer.SetRadius(_data.AttackRange);
                SetRangeVisualizerVisible(false);
            }

            if (_data.AttackSpeed > 0)
            {
                _fireCoolTime = 1f / _data.AttackSpeed;
            }

            if (!string.IsNullOrEmpty(_data.ProjectilePath))
            {
                ResourceManager.Instance.LoadAsset<GameObject>(_data.ProjectilePath, OnProjectileLoaded);
            }

            // 특수능력 ID를 기반으로 소금 디버프 데이터 로드
            if (!string.IsNullOrEmpty(_data.AbilityId))
            {
                if (_saltDebuff == null)
                {
                    _saltDebuff = gameObject.AddComponent<SaltDebuff>();
                }

                _saltDebuff.Initialize(_data.AbilityId);
            }
            else
            {
                _saltDebuff = null;
            }
        }
        else
        {
            Debug.LogError($"[{nameof(SaltTower)} 에러] GameDataManager에서 ID [{towerId}]에 해당하는 TowerData를 찾지 못했습니다.");
        }
    }

    protected override void Update()
    {
        // 원본 TowerBase의 에너미 감지(FindEnemiesInRange) 실행
        base.Update();

        RotateTowardsTarget();
        TryAttackTarget();
    }

    private void RotateTowardsTarget()
    {
        if (_detector == null)
        {
            return;
        }

        Transform targetEnemy = _detector.FindLeadEnemy();

        // Rotator 클래스에게 실시간 타겟을 세팅하고 회전 연산 위임
        _rotator.SetLookAt(targetEnemy);
        _rotator.Rotate(Time.deltaTime);
    }

    private void TryAttackTarget()
    {
        Transform targetEnemy = _detector.FindLeadEnemy();

        // 사거리 내에 조준된 대상이 없으면 공격 로직 중단
        if (targetEnemy == null)
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

        GameObject projectileInst = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
        SaltProjectile saltProjectile = projectileInst.GetComponent<SaltProjectile>();
        saltProjectile.Init(_damage, targetEnemy, _projectileSpeed);
        saltProjectile.SetupSaltAbility(_saltDebuff);

        _saltHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }

    private void OnProjectileLoaded(GameObject loadedPrefab)
    {
        _projectilePrefab = loadedPrefab;
    }
}
