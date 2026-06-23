using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SaltTower : TowerBase
{
    [Header("3D Range Visualizer Element")]
    [SerializeField] private GameObject _rangeObject; // 범위 표시용 실린더 오브젝트
    [SerializeField] private Transform _firePoint; // 투사체가 발사될 정확한 포지션

    private TowerRangeVisualizer _rangeVisualizer;
    private GameObject _projectilePrefab;
    private TowerData _currentData;

    private AsyncOperationHandle<GameObject> _projectileHandle;
    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;

    public void Initialize(string towerId)
    {
        _currentData = GameDataManager.Instance.GetData<TowerData>(towerId);

        if (_currentData != null)
        {
            if (_currentData.AttackSpeed > 0)
            {
                _fireCoolTime = 1f / _currentData.AttackSpeed;
            }

            // 어드레서블 비동기 로드 적용
            if (!string.IsNullOrEmpty(_currentData.ProjectilePath))
            {
                _projectileHandle = Addressables.LoadAssetAsync<GameObject>(_currentData.ProjectilePath);
                _projectileHandle.Completed += OnProjectileLoaded;
            }

            if (_rangeObject != null)
            {
                _rangeVisualizer = new TowerRangeVisualizer(_currentData.AttackRange, _rangeObject);
                _rangeVisualizer.HideRange();
            }
        }
    }

    private void OnProjectileLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _projectilePrefab = handle.Result;
        }
        else
        {
            Debug.LogWarning($"투사체 프리팹 로드 실패 {_currentData?.ProjectilePath}");
        }
    }

    protected override void Update()
    {
        base.Update();
        TryAttackTarget();
    }

    private void TryAttackTarget()
    {
        Transform targetEnemy = _detector.FindClosestEnemy();

        if (targetEnemy == null) return;
        if (Time.time - _lastFireTime < _fireCoolTime) return;
        if (_projectilePrefab == null) return;

        _lastFireTime = Time.time;

        Transform launchPoint = _firePoint != null ? _firePoint : transform;

        // 공용 AttackSystem으로 발사체만 날리고 타워의 역할은 끝
        AttackSystem.Attack(
            _projectilePrefab,
            launchPoint,
            targetEnemy,
            _currentData.AttackDamage,
            _currentData.ProjectileSpeed
        );
    }

    private void OnDestroy()
    {
        if (_projectileHandle.IsValid())
        {
            Addressables.Release(_projectileHandle);
        }
    }

    public void ToggleRangeVisualizer(bool show)
    {
        if (_rangeVisualizer == null) return;

        if (show) _rangeVisualizer.ShowRange();
        else _rangeVisualizer.HideRange();
    }
}