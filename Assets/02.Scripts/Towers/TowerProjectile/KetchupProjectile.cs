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
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 바닥 레이어인지 검사
        // 케찹 데칼 생성
        if (CheckIsGrounded(collision))
        {
            _decalSpawner.Spawn(collision.gameObject.transform.position, Vector3.up);
            Destroy(gameObject);
            return;
        }
    }

    private bool CheckIsGrounded(Collision collision)
    {
        return ((1 << collision.gameObject.layer) & _targetLayerMask) != 0;
    }
}
