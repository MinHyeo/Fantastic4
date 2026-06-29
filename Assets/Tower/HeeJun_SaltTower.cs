using DG.Tweening;
using UnityEngine;

public class HeeJun_SaltTower : TowerBase
{
    [Header("3D Range Visualizer Element")]
    //[SerializeField] private GameObject _rangeObject; // 범위 표시용 실린더 오브젝트
    [SerializeField] private Transform _saltHead; // 반동효과 줄 머리부분

    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectilePrefab;

    [SerializeField] private string _testTowerId;

    
    private TowerData _currentData;
    private SaltDebuff _saltDebuff;

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
            _detector.SetRange(_currentData.AttackRange);
            if (_rangeVisualizer != null) 
            {
                _rangeVisualizer.SetRadius(_currentData.AttackRange);
            }
            if (_currentData.AttackSpeed > 0)
            {
                _fireCoolTime = 1f / _currentData.AttackSpeed;
            }
            if (!string.IsNullOrEmpty(_currentData.AbilityId))
            {
                _saltDebuff = gameObject.AddComponent<SaltDebuff>();
                _saltDebuff.Initialize(_currentData.AbilityId);
            }
        }
        else
        {
            Debug.LogError($"[SaltTower 에러] ...");
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

        GameObject spawnedProjectile = Instantiate(_projectilePrefab, launchPoint.position, launchPoint.rotation);
        Projectile projectile = spawnedProjectile.GetComponent<Projectile>();
        if (projectile == null)
        {
            Debug.LogWarning($"{_projectilePrefab.name}에 Projectile 스크립트 없음");
            return;
        }

        projectile.Init(_damage, targetEnemy, _projectileSpeed);

        if (_saltHead != null)  
        {
            _saltHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
        }
    }

    public override void ToggleRangeVisualizer()
    {
        if (_rangeVisualizer == null)
        {
            return;
        }

        _isToggleRangeVisualizer = _isToggleRangeVisualizer ? false : true;
        _rangeVisualizer.SetVisible(_isToggleRangeVisualizer);
    }
}
