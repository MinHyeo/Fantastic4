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


    protected override void Update()
    {
        Move();
    }

    protected override void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, (_projectileSpeed * Time.deltaTime));
    }

    ///<summary>
    /// 충돌 시 호출됩니다.
    ///</summary>
    private void OnTriggerEnter(Collider collider)
    {
        if (CheckIsEnemyCollider(collider))
        {
            Destroy(gameObject);
            // TODO : 적에 대한 공격 판정

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
}
