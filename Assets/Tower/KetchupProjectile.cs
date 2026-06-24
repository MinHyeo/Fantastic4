using UnityEngine;

public class KetchupProjectile : Projectile
{
    ///<summary>
    /// 케찹 데칼 생성기
    ///</summary>
    [SerializeField] private KetchupDecalSpawner _decalSpawner;

    ///<summary>
    /// 바닥 레이어 마스크
    ///</summary>
    [SerializeField] private LayerMask _groundLayerMask;

    ///<summary>
    /// 충돌 후 투사체 삭제 여부
    ///</summary>
    [SerializeField] private bool _destroyOnHit = true;


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
        // 충돌한 오브젝트가 바닥 레이어인지 검사
        if (CheckIsGrounded(collision))
        {
            return;
        }

        // 충돌 지점 가져오기
        ContactPoint contact = collision.GetContact(0);

        // 케찹 데칼 생성
        _decalSpawner.Spawn(contact.point, contact.normal);

        // 투사체 삭제
        if (_destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    private bool CheckIsGrounded(Collision collision)
    {
        return ((1 << collision.gameObject.layer) & _groundLayerMask) != 0;
    }
}
