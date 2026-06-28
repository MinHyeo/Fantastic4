using UnityEngine;

public class SaltTower : TowerBase
{
    [Header("3D Range Visualizer Element")]
    [SerializeField] private GameObject _rangeObject; // 범위 표시용 실린더 오브젝트
    [SerializeField] private Transform _firePoint; // 투사체가 발사될 정확한 포지션

    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectilePrefab;

    [SerializeField] private string _testTowerId;

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

            if (_data.AttackSpeed > 0)
            {
                _fireCoolTime = 1f / _data.AttackSpeed;
            }

            // 특수능력 ID를 기반으로 소금 디버프 데이터 로드
            if (!string.IsNullOrEmpty(_data.AbilityId))
            {
                _saltDebuff = gameObject.AddComponent<SaltDebuff>();
                _saltDebuff.Initialize(_data.AbilityId);
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

        Transform targetEnemy = _detector.FindClosestEnemy();

        // Rotator 클래스에게 실시간 타겟을 세팅하고 회전 연산 위임
        _rotator.SetLookAt(targetEnemy);
        _rotator.Rotate(Time.deltaTime);
    }

    private void TryAttackTarget()
    {
        Transform targetEnemy = _detector.FindClosestEnemy();

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

        // 발사 위치 예외 처리 (인스펙터 미지정 시 타워 본체 포지션 사용)
        Transform launchPoint = _firePoint != null ? _firePoint : transform;

        AttackSystem.Attack(_projectilePrefab, launchPoint, targetEnemy, 
            _damage,
            _projectileSpeed
        );
    }
}
