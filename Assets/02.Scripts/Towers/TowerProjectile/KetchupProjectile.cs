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

    private bool _isHit;


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

        transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, (_projectileSpeed * Time.deltaTime));

        float distance = Vector3.Distance(_targetTransform.position, transform.position);
        if (distance < 0.1f)
        {
            Hit(transform.position, _targetTransform.gameObject);
        }
    }

    ///<summary>
    /// 충돌 시 호출됩니다.
    ///</summary>
    private void OnTriggerEnter(Collider collider)
    {
        if (CheckIsEnemyCollider(collider))
        {
            Hit(transform.position, collider.gameObject);

            return;
        }
    }

    private bool CheckIsEnemyCollider(Collider collider)
    {
        return ((1 << collider.gameObject.layer) & _targetLayerMask) != 0;
    }

    private bool CheckIsGrounded(Collision collision)
    {
        return ((1 << collision.gameObject.layer) & _targetLayerMask) != 0;
    }

    private void Hit(Vector3 position, GameObject enemyTarget)
    {
        if (_isHit)
        {
            return;
        }

        _isHit = true;
        BattleManager.Instance.AttackToEnemy(enemyTarget, _damage);
        SpawnDecal(position);
        Destroy(gameObject);
    }

    private void SpawnDecal(Vector3 position)
    {
        if (_decalSpawner == null)
        {
            return;
        }

        float groundY = _targetTransform != null ? _targetTransform.position.y : position.y;
        Vector3 decalPosition = position;
        decalPosition.y = groundY;

        _decalSpawner.Spawn(decalPosition, Vector3.up);
    }
}
