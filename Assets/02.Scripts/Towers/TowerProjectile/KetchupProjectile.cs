using Cysharp.Threading.Tasks;
using UnityEngine;

public class KetchupProjectile : Projectile
{
    ///<summary>
    /// 케찹 데칼 생성기
    ///</summary>
    [SerializeField] private KetchupDecalSpawner _decalSpawner;

    ///<summary>
    /// 충돌 시 효과를 일으킬 레이어
    ///</summary>
    [SerializeField] private LayerMask _targetLayerMask;

    private string _abilityId;

    public override void Init(float damage, Transform targetTransform, float projectileSpeed, string abilityId = "")
    {
        _damage = damage;
        _targetTransform = targetTransform;
        _projectileSpeed = projectileSpeed;

        if (string.IsNullOrEmpty(abilityId) == false)
        {
            // TODO : 여기서 전달
            _abilityId = abilityId;
        }
    }

    protected override void Update()
    {
        Move();
    }

    protected override void Move()
    {
        Debug.LogWarning("케찹 Move호출");
        if (_targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, (_projectileSpeed * Time.deltaTime));
        float distance = Vector3.Distance(_targetTransform.position, transform.position);
        Debug.Log($"케찹 distance: {distance}");
        if (distance < 0.1f)
        {
            float groundY = _targetTransform.position.y;
            Vector3 decalPosition = transform.position;
            decalPosition.y = groundY + 0.01f;   // 적 발밑 높이

            if (_decalSpawner != null)
            {
                _decalSpawner.Spawn(decalPosition, Vector3.up);
            }

            // 케찹 어빌리티 장판 생성
            if (string.IsNullOrEmpty(_abilityId) == false)
            {
                GameObjectManager.Instance.CreateAbilityObject(_abilityId, decalPosition).Forget();
            }

            Debug.LogWarning("투사체 피격");
            Destroy(gameObject);
        }
    }
}
