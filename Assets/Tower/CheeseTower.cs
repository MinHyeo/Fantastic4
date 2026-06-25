using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CheeseTower : TowerBase
{
    [SerializeField] private Transform _headBase;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private float _upgradeScaleMultiplier = 1.5f;




    [SerializeField] private string _testTowerId; // 동작 테스트용

    private GameObject _projectilePrefab;
    private float _damage;
    private float _projectileSpeed;

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
                _fireCoolTime = (1f / _currentData.AttackSpeed);
            }

            // 임시 ProjectilePath 하드코딩, 엑셀 데이터 채워지면 삭제해야함
            if (string.IsNullOrEmpty(_currentData.ProjectilePath))
            {
                _currentData.ProjectilePath = "Prefab/Projectile/cheese";
            }

            if (!string.IsNullOrEmpty(_currentData.ProjectilePath))
            {
                ResourceManager.Inst.LoadAsset<GameObject>(_currentData.ProjectilePath, OnProjectileLoaded);
            }
        }
    }

    private void OnProjectileLoaded(GameObject loadedPrefab)
    {
        _projectilePrefab = loadedPrefab;
    }

    public void Upgrade()
    {
        if(_currentData == null)
        {
            return;
        }

        string nextId = _currentData.UpgradeId;
        if (string.IsNullOrEmpty(nextId))
        {
            Debug.Log("이미 최대 강화상태임");
            return;
        }

        Initialize(nextId);
        ApplyUpgradeVisual();
        Debug.LogWarning($"강화 후 데미지{_currentData.AttackDamage}, 사거리 {_currentData.AttackRange}");
    }

    private void ApplyUpgradeVisual()
    {
        if (_headBase == null)
        {
            return;
        }

        _headBase.localScale *= _upgradeScaleMultiplier;
                   
    }
}
