using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CheeseTower : TowerBase
{
    [SerializeField] private Transform _headBase;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private Transform _cheeseHead;





    [SerializeField] private string _testTowerId; // 동작 테스트용

    private GameObject _projectilePrefab;
    private float _damage;
    private float _projectileSpeed;

    private float _lastFireTime;
    private float _fireCoolTime = 1.0f;

    protected override void Start() // 동작 테스트용, 매니저 연동후 삭제
    {
        base.Start();
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

        _cheeseHead.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }

    
    public override void Init(string towerId)
    {
        _data = GameDataManager.Instance.GetData<TowerData>(towerId);

        if (_data != null)
        {
            _damage = _data.AttackDamage;
            _projectileSpeed = _data.ProjectileSpeed;
            _detector.DetectionRange = _data.AttackRange;

            if (_rangeVisualizer != null)
            {
                _rangeVisualizer.SetRange(_data.AttackRange);
            }

            if (_data.AttackSpeed > 0)
            {
                _fireCoolTime = (1f / _data.AttackSpeed);
            }

            if (!string.IsNullOrEmpty(_data.ProjectilePath))
            {
                ResourceManager.Instance.LoadAsset<GameObject>(_data.ProjectilePath, OnProjectileLoaded);
            }
        }
    }

    private void OnProjectileLoaded(GameObject loadedPrefab)
    {
        _projectilePrefab = loadedPrefab;
    }
}
