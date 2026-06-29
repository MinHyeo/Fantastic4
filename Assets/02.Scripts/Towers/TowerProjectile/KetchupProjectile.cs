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

    public override void Init(float damage, Transform targetTransform, float projectileSpeed)
    {
        _damage = damage;
        _targetTransform = targetTransform;
        _projectileSpeed = projectileSpeed;
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
            decalPosition.y = groundY;   // 적 발밑 높이
            if (_decalSpawner != null)
            {
                _decalSpawner.Spawn(decalPosition, Vector3.up);
            }
            Debug.LogWarning("투사체 피격");
            Destroy(gameObject);
        }
    }
}
