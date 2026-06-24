using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CheeseTower : TowerBase
{
    [SerializeField] private Transform _headBase;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _rotationSpeed = 360f;

    [SerializeField] private float _fireSpeed;
    [SerializeField] private string _testTowerId; // 동작 테스트용

    private GameObject _projectilePrefab;
    private float _damage;
    private float _projectileSpeed;

    private AsyncOperationHandle<GameObject> _projectileHandle;
    private TowerData _currentData;
    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;

    private void Start() // 동작 테스트용, 매니저 연동후 삭제
    {
        Initialize(_testTowerId);
    }
    protected override void Update()
    {
        base.Update();

        Transform targetTransform = _detector.FindClosestEnemy();

        if (targetTransform == null)
        {
            return;
        }

        Vector3 direction = targetTransform.position - _headBase.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _headBase.rotation = Quaternion.RotateTowards(_headBase.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

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

            // 임시 ProjectilePath 하드코딩, 엑셀 데이터 채워지면 삭제해야함
            if (string.IsNullOrEmpty(_currentData.ProjectilePath))
            {
                _currentData.ProjectilePath = "Prefab/Projectile/cheese";
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
