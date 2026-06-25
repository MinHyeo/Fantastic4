using UnityEngine;

public class PepperProjectile : Projectile
{
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
    private void OnCollisionEnter(Collision collision)
    {
        if (CheckIsEnemyCollider(collision))
        {
            Destroy(gameObject);
            // TODO : 적에 대한 공격 판정
            return;
        }
    }

    private bool CheckIsEnemyCollider(Collision collision)
    {
        return ((1 << collision.gameObject.layer) & _targetLayerMask) != 0;
    }
}
