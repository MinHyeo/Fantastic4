using UnityEngine;

public class SaltTower : TowerBase
{
    [Header("3D Range Visualizer Element")]
    [SerializeField] private GameObject _rangeObject; // 범위 표시용 실린더 오브젝트
    [SerializeField] private Transform _firePoint; // 투사체가 발사될 정확한 포지션

    private TowerRangeVisualizer _rangeVisualizer;
    private GameObject _projectilePrefab;
    private AbilityData _abilityData;

    private TowerData _currentData;
    private float _attackTimer = 0f;

    public void Initialize(string towerId)
    {
        // 1. 데이터 매니저에서 타워의 기본 스펙 정보 가져오기
        _currentData = GameDataManager.Instance.GetData<TowerData>(towerId);

        if (_currentData != null)
        {
            // 2. 프리팹 경로를 기반으로 발사체 리소스 로드
            if (!string.IsNullOrEmpty(_currentData.ProjectilePath))
            {
                _projectilePrefab = Resources.Load<GameObject>(_currentData.ProjectilePath);
            }

            // 3. 특수능력 ID를 기반으로 소금 디버프 데이터 로드
            if (!string.IsNullOrEmpty(_currentData.AbilityId))
            {
                _abilityData = GameDataManager.Instance.GetData<AbilityData>(_currentData.AbilityId);
            }

            // 4. 팀원의 시각화 클래스를 생성하여 사거리 적용
            if (_rangeObject != null)
            {
                _rangeVisualizer = new TowerRangeVisualizer(_currentData.AttackRange, _rangeObject);
                _rangeVisualizer.HideRange();
            }
        }
    }

    protected override void Update()
    {
        // 원본 TowerBase의 에너미 감지(FindEnemiesInRange) 실행
        base.Update();

        // 공격 쿨타임 타이머 계산
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
        }

        // 쿨타임이 완료되면 사거리 내 대상을 조준하고 공격
        if (_attackTimer <= 0f)
        {
            TryAttackTarget();
        }
    }

    private void TryAttackTarget()
    {
        Transform targetEnemy = _detector.FindClosestEnemy();

        // 사거리 내에 조준된 대상이 없으면 공격 로직 중단
        if (targetEnemy == null) return;

        float attackCooldown = _currentData != null && _currentData.AttackSpeed > 0
            ? 1f / _currentData.AttackSpeed
            : 1f;
        _attackTimer = attackCooldown;

        if (_projectilePrefab != null)
        {
            // 발사 위치 예외 처리 (인스펙터 미지정 시 타워 본체 포지션 사용)
            Transform launchPoint = _firePoint != null ? _firePoint : transform;

            AttackSystem.Attack(
                _projectilePrefab,
                launchPoint,
                targetEnemy,
                _currentData.AttackDamage,
                _currentData.ProjectileSpeed
            );

            // 런타임에 막 생성된 투사체를 탐색하여 소금 타워의 특수 능력 데이터 주입
            Collider[] hitProjectiles = Physics.OverlapSphere(launchPoint.position, 0.5f);
            foreach (var col in hitProjectiles)
            {
                var saltProj = col.GetComponent<SaltTowerProjectile>();
                if (saltProj != null && !saltProj.IsInitialized)
                {
                    saltProj.SetupSaltAbility(_abilityData);
                    break;
                }
            }
        }
    }

    public void ToggleRangeVisualizer(bool show)
    {
        if (_rangeVisualizer == null) return;

        if (show) _rangeVisualizer.ShowRange();
        else _rangeVisualizer.HideRange();
    }
}