using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class OilTower : TowerBase
{
    [Header(nameof(OilTower))]

    private GameObject _projectilePrefab;
    [SerializeField] private Transform _oilHead;

    //private OilSlickAura _oilSlickAura; // 기름 타워 전용 지속 데미지(DoT) 장판/디버프 능력 컴포넌트

    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;
    private float _damage;
    private float _projectileSpeed;

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
                _fireCoolTime = 1f / _data.AttackSpeed;
            }

            // 특수능력 ID를 기반으로 기름 장판/디버프 능력 데이터 로드
            //if (!string.IsNullOrEmpty(_data.AbilityId))
            //{
            //    _oilSlickAura = gameObject.AddComponent<OilSlickAura>();
            //    _oilSlickAura.Init(_data.AbilityId);
            //}

            if (!string.IsNullOrEmpty(_data.ProjectilePath))
            {
                ResourceManager.Instance.LoadAsset<GameObject>(_data.ProjectilePath, OnProjectileLoaded);
            }
        }
        else
        {
            Debug.LogError($"[{nameof(OilTower)} 에러] GameDataManager에서 ID [{towerId}]에 해당하는 TowerData를 찾지 못했습니다.");
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
        Collider[] targetEnemies = _detector.FindEnemiesInRange();

        // 사거리 내에 공격할 대상이 없으면 공격 로직 중단
        if (targetEnemies.Length == 0)
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

        // 발사 위치 예외 처리 (인스펙터 미지정 시 타워 본체 포지션 사용)
        Transform launchPoint = _firePoint != null ? _firePoint : transform;
        HashSet<GameObject> firedTargets = new HashSet<GameObject>();

        foreach (Collider targetEnemy in targetEnemies)
        {
            if (targetEnemy == null)
            {
                continue;
            }

            // 적 하나에 콜라이더가 여러 개 잡혀도 투사체는 한 번만 발사합니다.
            if (firedTargets.Add(targetEnemy.gameObject) == false)
            {
                continue;
            }

            GameObject spawnedProjectile = Instantiate(_projectilePrefab, launchPoint.position, launchPoint.rotation);
            Projectile projectile = spawnedProjectile.GetComponent<Projectile>();
            if (projectile == null)
            {
                Debug.LogWarning($"{_projectilePrefab.name}에 Projectile 스크립트 없음");
                Destroy(spawnedProjectile);
                continue;
            }

            projectile.Init(_damage, targetEnemy.transform, _projectileSpeed);
        }

        // 공격 시 애님
        _oilHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }

    private void OnProjectileLoaded(GameObject loadedPrefab)
    {
        _projectilePrefab = loadedPrefab;
    }
}
