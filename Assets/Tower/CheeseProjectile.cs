using DG.Tweening;
using UnityEngine;

public class CheeseProjectile : Projectile
{
    [SerializeField] private GameObject _burstEffect;

    ///<summary>
    /// 충돌 시 효과를 일으킬 레이어
    ///</summary>
    [SerializeField] private LayerMask _targetLayerMask;

    public override void Init(float damage, Transform targetTransform, float projectileSpeed, string abilityId = "")
    {
        _damage = damage;
        _targetTransform = targetTransform;
        _projectileSpeed = projectileSpeed;

        // 후추 투사체는 전방 축 기준으로 회전하는 이펙트를 사용
        transform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
    }

    protected override void Update()
    {
        Move();

        if (IsProjectileLifetimeExpired() == true)
        {
            Destroy(gameObject);
        }
        else
        {
            SetPrevPos();
        }
    }

    protected override void Move()
    {
        if (_targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, (_projectileSpeed * Time.deltaTime));
    }

    ///<summary>
    /// 충돌 시 호출됩니다.
    ///</summary>
    private void OnTriggerEnter(Collider collider)
    {
        if (CheckIsEnemyCollider(collider))
        {
            BattleManager.Instance.AttackToEnemy(collider.gameObject, _damage);
            SpawnBurstEffect();
            Destroy(gameObject);

            return;
        }
    }

    private bool CheckIsEnemyCollider(Collider collider)
    {
        return ((1 << collider.gameObject.layer) & _targetLayerMask) != 0;
    }

    private void SpawnBurstEffect()
    {
        if (_burstEffect == null)
        {
            return;
        }

        Instantiate(_burstEffect, transform.position, Quaternion.identity);
    }

    protected override bool IsProjectileLifetimeExpired()
    {
        bool result = base.IsProjectileLifetimeExpired();

        result = Vector3.Distance(_prevPos, transform.position) < 0.001f ||
                 _targetTransform == null;

        return result;
    }
}
