using UnityEngine;

public class OilTower : TowerBase
{
    [Header("3D Range Visualizer Element")]
    [SerializeField] private GameObject _rangeObject; // 범위 표시용 실린더 오브젝트
    [SerializeField] private Transform _firePoint; // 투사체가 발사될 정확한 포지션

    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectilePrefab;

    [SerializeField] private string _testTowerId;

    private TowerRangeVisualizer _rangeVisualizer;
    private TowerData _currentData;
    //private OilSlickAura _oilSlickAura; // 기름 타워 전용 지속 데미지(DoT) 장판/디버프 능력 컴포넌트

    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;
    private float _damage;
    private float _projectileSpeed;

    private void Start()
    {
        Initialize(_testTowerId);
    }

    public void Initialize(string towerId)
    {
        _currentData = GameDataManager.Instance.GetData<TowerData>(towerId);

        if (_currentData != null)
        {
            _damage = _currentData.AttackDamage;
            _projectileSpeed = _currentData.ProjectileSpeed;
            _detector.DetectionRange = _currentData.AttackRange;

            if (_currentData.AttackSpeed > 0)
            {
                _fireCoolTime = 1f / _currentData.AttackSpeed;
            }

            // 특수능력 ID를 기반으로 기름 장판/디버프 능력 데이터 로드
            //if (!string.IsNullOrEmpty(_data.AbilityId))
            //{
            //    _oilSlickAura = gameObject.AddComponent<OilSlickAura>();
            //    _oilSlickAura.Init(_data.AbilityId);
            //}

            if (_rangeObject != null)
            {
                _rangeVisualizer = new TowerRangeVisualizer(_currentData.AttackRange, _rangeObject);
                _rangeVisualizer.HideRange();
            }
        }
        else
        {
            Debug.LogError($"[OilTower 에러] GameDataManager에서 ID [{towerId}]에 해당하는 TowerData를 찾지 못했습니다.");
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

    public void ToggleRangeVisualizer(bool show)
    {
        if (_rangeVisualizer == null)
        {
            return;
        }

        if (show)
        {
            _rangeVisualizer.ShowRange();
        }
        else
        {
            _rangeVisualizer.HideRange();
        }
    }
}
