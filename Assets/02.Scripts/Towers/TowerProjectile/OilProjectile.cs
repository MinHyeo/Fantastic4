using Cysharp.Threading.Tasks;
using UnityEngine;

public class OilProjectile : Projectile
{
    [SerializeField] private GameObject _burstEffect;
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private float _arcHeight = 3f;
    [SerializeField] private float _impactDistance = 0.1f;

    private AbilityData _oilAbility;
    private string _abilityId;
    private Vector3 _startPosition;
    private float _elapsedTime;
    private float _flightDuration;
    private bool _isHit;

    public bool IsInitialized => _oilAbility != null;

    public override void Init(float damage, Transform targetTransform, float projectileSpeed, string abilityId = "")
    {
        base.Init(damage, targetTransform, projectileSpeed);

        _startPosition = transform.position;
        _elapsedTime = 0f;
        _isHit = false;

        float distance = _targetTransform != null ? Vector3.Distance(_startPosition, _targetTransform.position) : 0f;
        _flightDuration = Mathf.Max(distance / Mathf.Max(_projectileSpeed, 0.01f), 0.01f);

        if (string.IsNullOrEmpty(abilityId) == false)
        {
            _abilityId = abilityId;
        }
    }

    public void SetupKetchupAbility(AbilityData abilityData)
    {
        _oilAbility = abilityData;
    }

    protected override void Update()
    {
        Move();
    }

    protected override void Move()
    {
        if (_targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        _elapsedTime += Time.deltaTime;

        float progress = Mathf.Clamp01(_elapsedTime / _flightDuration);
        Vector3 targetPosition = _targetTransform.position;
        Vector3 nextPosition = Vector3.Lerp(_startPosition, targetPosition, progress);

        // 위로 솟은 뒤 내려찍히는 화산 분출형 궤적입니다.
        nextPosition.y += Mathf.Sin(progress * Mathf.PI) * _arcHeight;
        transform.position = nextPosition;

        if (progress >= 1f || Vector3.Distance(transform.position, targetPosition) < _impactDistance)
        {
            Hit(_targetTransform.gameObject);
        }
    }

    ///<summary>
    /// 충돌 시 호출됩니다.
    ///</summary>
    private void OnTriggerEnter(Collider collider)
    {
        if (CheckIsEnemyCollider(collider))
        {
            Hit(collider.gameObject);

            return;
        }
    }

    private bool CheckIsEnemyCollider(Collider collider)
    {
        return ((1 << collider.gameObject.layer) & _targetLayerMask) != 0;
    }

    private void Hit(GameObject target)
    {
        if (_isHit)
        {
            return;
        }

        _isHit = true;
        BattleManager.Instance.AttackToEnemy(target, _damage);
        CreateOilField(target);
        SpawnBurstEffect();
        Destroy(gameObject);
    }

    private void CreateOilField(GameObject target)
    {
        if (string.IsNullOrEmpty(_abilityId))
        {
            return;
        }

        // 오일 투사체가 맞은 위치에 장판 어빌리티를 생성합니다.
        Vector3 fieldPosition = transform.position;
        if (target != null)
        {
            fieldPosition.y = target.transform.position.y + 0.01f;
        }

        GameObjectManager.Instance.CreateAbilityObject(_abilityId, fieldPosition).Forget();
    }

    private void SpawnBurstEffect()
    {
        if (_burstEffect == null)
        {
            return;
        }

        Instantiate(_burstEffect, transform.position, Quaternion.identity);
    }
}
