using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CheeseTower : TowerBase
{
    [Header("임시 동작 테스트용")]
     private GameObject _projectilePrefab;
     private float _damage = 10.0f;
     private float _projectileSpeed = 5.0f;
    [SerializeField] private Transform _firePoint;

    private AsyncOperationHandle<GameObject> _projectileHandle;
    private TowerData _currentData;
    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;

    protected override void Update()
    {
        base.Update();

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

        AttackSystem.Attack(_projectilePrefab, _firePoint, targetTransform, _damage, _projectileSpeed);
        _lastFireTime = Time.time;
    }

    private void OnDestroy()
    {
        if (_projectileHandle.IsValid())
        {
            Addressables.Release(_projectileHandle);
        }
    }
    public void Initialize(string towerId)
    {
        _currentData = GameDataManager.Instance.GetData<TowerData>(towerId);

        if (_currentData != null)
        {
            _damage = _currentData.AttackDamage;
            _projectileSpeed = _currentData.ProjectileSpeed;

            if (_currentData.AttackSpeed > 0)
            {
                _fireCoolTime = (1f / _currentData.AttackSpeed);
            }

            if (!string.IsNullOrEmpty(_currentData.ProjectilePath))
            {
                _projectileHandle = Addressables.LoadAssetAsync<GameObject>(_currentData.ProjectilePath);
                _projectileHandle.Completed += OnProjectileLoaded;
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
            Debug.LogWarning($"투사체 프리팹 로드 실패 {_currentData.ProjectilePath}");
        }
    }
}
