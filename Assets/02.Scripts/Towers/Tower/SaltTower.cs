using UnityEngine;

public class SaltTower : TowerBase
{
    [Header("3D Range Visualizer Element")]
    [SerializeField] private GameObject _rangeObject; // 범위 표시용 실린더 오브젝트
    [SerializeField] private Transform _firePoint; // 투사체가 발사될 정확한 포지션

    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectilePrefab;

    private TowerRangeVisualizer _rangeVisualizer;
    private TowerData _currentData;
    private SaltDebuff _saltDebuff;

    [Header("Rotation Settings")]
    [SerializeField] private Transform _headTransform;

    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;

    public void Initialize(string towerId)
    {
        //_currentData = GameDataManager.Instance.GetData<TowerData>("Tower_04_Level1");

        //if (_currentData != null)
        //{
        //    if (_currentData.AttackSpeed > 0)
        //    {
        //        _fireCoolTime = 1f / _currentData.AttackSpeed;
        //    }

        //    // 특수능력 ID를 기반으로 소금 디버프 데이터 로드
        //    if (!string.IsNullOrEmpty(_currentData.AbilityId))
        //    {
        //        _saltDebuff = gameObject.AddComponent<SaltDebuff>();
        //        _saltDebuff.Initialize(_currentData.AbilityId);
        //    }

        //    if (_rangeObject != null)
        //    {
        //        _rangeVisualizer = new TowerRangeVisualizer(_currentData.AttackRange, _rangeObject);
        //        _rangeVisualizer.HideRange();
        //    }
        //}

        _fireCoolTime = 1f / 2f; // 공격 속도 2 반영 (0.5초당 1번 발사)

        if (_rangeObject != null)
        {
            _rangeVisualizer = new TowerRangeVisualizer(2.0f, _rangeObject); // 임시 사거리 2 수치 적용
            _rangeVisualizer.HideRange();
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

        if (targetEnemy == null)
        {
            return;
        }

        // 타워 몸통(transform)과 적의 위치 차이 계산 (Y축 값을 같게 만들어 위아래로 꺾이는 현상 방지)
        Vector3 direction = targetEnemy.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            // 적을 바라보는 쿼터니언 각도 계산 및 회전 적용 (Quaternion.LookRotation)
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 즉시 휙 도는 대신 초당 10의 속도로 부드럽게 몸통 회전 연출
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
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

        AttackSystem.Attack(
            _projectilePrefab,
            launchPoint,
            targetEnemy,
            //_currentData.AttackDamage,
            //_currentData.ProjectileSpeed,
            1.0f,
            5.0f
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
